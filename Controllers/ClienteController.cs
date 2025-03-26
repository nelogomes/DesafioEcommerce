using DesafioEcommerce.Data;
using DesafioEcommerce.DTOs;
using DesafioEcommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
	private readonly EcommerceDbContext _context;

	public ClientesController(EcommerceDbContext context)
	{
		_context = context;
	}

	// POST: api/clientes
	[HttpPost]
	public async Task<IActionResult> CriarCliente([FromBody] ClienteDTO clienteDto)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var cliente = new Cliente
		{
			Nome = clienteDto.Nome,
			Cpf = clienteDto.Cpf,
			Categoria = clienteDto.Categoria
		};

		_context.Clientes.Add(cliente);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(ObterClientePorId), new { id = cliente.ClienteId }, cliente);
	}

	// GET: api/clientes/{id}
	[HttpGet("{id}")]
	public async Task<IActionResult> ObterClientePorId(Guid id)
	{
		var cliente = await _context.Clientes.FindAsync(id);
		if (cliente == null)
		{
			return NotFound();
		}
		return Ok(cliente);
	}
}
