namespace DesafioEcommerce.DTOs
{
	public class PedidoDTO
	{
		public Guid Identificador { get; set; }
		public DateTime DataVenda { get; set; }
		public ClienteDTO Cliente { get; set; } = default!;
		public List<ItemPedidoDTO> Itens { get; set; } = new();
	}

	public class ClienteDTO
	{
		public Guid ClienteId { get; set; }
		public string Nome { get; set; } = default!;
		public string Cpf { get; set; } = default!;
		public string Categoria { get; set; } = default!; // REGULAR, PREMIUM, VIP
	}

	public class ItemPedidoDTO
	{
		public int ProdutoId { get; set; }
		public string Descricao { get; set; } = default!;
		public decimal Quantidade { get; set; }
		public decimal PrecoUnitario { get; set; }
	}
}
