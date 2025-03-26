namespace DesafioEcommerce.Models
{
	public class Cliente
	{
		public Guid ClienteId { get; set; }
		public string Nome { get; set; } = default!;
		public string Cpf { get; set; } = default!;
		public string Categoria { get; set; } = default!; // REGULAR, PREMIUM, VIP
	}
}
