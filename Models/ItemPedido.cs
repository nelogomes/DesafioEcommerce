namespace DesafioEcommerce.Models
{
	public class ItemPedido
	{
		public int Id { get; set; }
		public Guid PedidoId { get; set; }
		public int ProdutoId { get; set; }
		public string Descricao { get; set; } = default!;
		public decimal Quantidade { get; set; }
		public decimal PrecoUnitario { get; set; }
	}
}
