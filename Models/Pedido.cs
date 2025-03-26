namespace DesafioEcommerce.Models
{
	public class Pedido
	{
		public Guid Id { get; set; }
		public DateTime DataVenda { get; set; }
		public Cliente Cliente { get; set; } = default!;
		public List<ItemPedido> Itens { get; set; } = new();
		public decimal SubTotal { get; set; }
		public decimal Descontos { get; set; }
		public decimal ValorTotal { get; set; }
		public string Status { get; set; } = "PENDENTE"; // PENDENTE ou CONCLUIDO
	}
}
