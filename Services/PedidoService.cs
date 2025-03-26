using DesafioEcommerce.Data;
using DesafioEcommerce.Models;

namespace DesafioEcommerce.Services
{
	public class PedidoService
	{
		private readonly EcommerceDbContext _context;

		public PedidoService(EcommerceDbContext context)
		{
			_context = context;
		}

		public async Task<Pedido> ProcessarPedidoAsync(DTOs.PedidoDTO pedidoDto)
		{
			Guid novoGuidPedido = Guid.NewGuid();
			Guid novoGuidCliente = Guid.NewGuid();

			decimal subTotal = pedidoDto.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);
			decimal desconto = CalcularDesconto(pedidoDto.Cliente.Categoria, subTotal);

			var pedido = new Pedido
			{
				Id = novoGuidPedido,
				DataVenda = DateTime.Now,
				Cliente = new Cliente
				{
					ClienteId = novoGuidCliente,
					Nome = pedidoDto.Cliente.Nome,
					Cpf = pedidoDto.Cliente.Cpf,
					Categoria = pedidoDto.Cliente.Categoria
				},
				Itens = pedidoDto.Itens.Select(i => new ItemPedido
				{
					ProdutoId = i.ProdutoId,
					Descricao = i.Descricao,
					Quantidade = i.Quantidade,
					PrecoUnitario = i.PrecoUnitario
				}).ToList(),
				SubTotal = subTotal,
				Descontos = desconto,
				ValorTotal = subTotal - desconto,
				Status = "PENDENTE"
			};

			// Salvar no banco de dados
			_context.Pedidos.Add(pedido);
			await _context.SaveChangesAsync();

			return pedido;
		}

		private decimal CalcularDesconto(string categoria, decimal subTotal)
		{
			return categoria switch
			{
				"REGULAR" => subTotal > 500 ? subTotal * 0.05m : 0,
				"PREMIUM" => subTotal > 300 ? subTotal * 0.10m : 0,
				"VIP" => subTotal * 0.15m,
				_ => 0
			};
		}
	}
}
