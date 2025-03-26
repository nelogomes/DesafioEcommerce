using DesafioEcommerce.Data;
using DesafioEcommerce.Models;
using DesafioEcommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestSharp;


namespace DesafioEcommerce.Controllers
{
	[Route("api/pedidos")]
	[ApiController]
	public class PedidoController : ControllerBase
	{
		private readonly PedidoService _pedidoService;
		private readonly EcommerceDbContext _context;

		public PedidoController(PedidoService pedidoService, EcommerceDbContext context)
		{
			_pedidoService = pedidoService;
			_context = context;  
		}

		[HttpPost]
		public async Task<ActionResult<Pedido>> CriarPedido([FromBody] DTOs.PedidoDTO pedidoDto)
		{
			if (pedidoDto == null || !pedidoDto.Itens.Any())
			{
				return BadRequest("Pedido inválido.");
			}

			var pedido = await _pedidoService.ProcessarPedidoAsync(pedidoDto);


			// Retorno
			var body = new
			{
				identificador = pedido.Id,
				subTotal = pedido.SubTotal,
				descontos = pedido.Descontos,
				valorTotal = pedido.ValorTotal,
				itens = pedido.Itens.Select(i => new
				{
					quantidade = i.Quantidade,
					precoUnitario = i.PrecoUnitario
				}).ToList()
			};

			var options = new RestClientOptions("https://sti3-faturamento.azurewebsites.net");
			var client = new RestClient(options);
			var request = new RestRequest("/api/vendas", Method.Post);
			request.AddHeader("email", "nelogomes@hotmail.com");
			request.AddHeader("Content-Type", "application/json");
			request.AddJsonBody(body);

			RestResponse response = await client.ExecuteAsync(request);

			if (response.IsSuccessful)
			{
				pedido.Status = "CONCLUIDO";
				await AtualizarPedidoAsync(pedido);
			}
			else
			{
				return StatusCode((int)response.StatusCode, "Erro ao faturar o pedido.");
			}

			return Ok(pedido.Id);
		}

		public async Task<Pedido> AtualizarPedidoAsync(Pedido pedido)
		{
			var pedidoExistente = await _context.Pedidos.FindAsync(pedido.Id);

			if (pedidoExistente == null)
			{
				throw new Exception("Pedido não encontrado.");
			}

			pedidoExistente.Status = pedido.Status ?? pedidoExistente.Status; 

			// pedidoExistente.SubTotal = pedido.SubTotal;
			// pedidoExistente.ValorTotal = pedido.ValorTotal;

			_context.Pedidos.Update(pedidoExistente);
			await _context.SaveChangesAsync();

			return pedidoExistente;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Pedido>>> GetTodosPedidos()
		{
			var pedidos = await _context.Pedidos.ToListAsync();

			// Verifica se existem pedidos
			if (pedidos == null || !pedidos.Any())
			{
				return NotFound("Nenhum pedido encontrado.");
			}

			return Ok(pedidos);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Pedido>> GetPedidoPorId(Guid id)
		{
			var pedido = await _context.Pedidos
				.FirstOrDefaultAsync(p => p.Id == id);

			if (pedido == null)
			{
				return NotFound($"Pedido {id} não encontrado.");
			}

			return Ok(pedido);
		}
	}
}
