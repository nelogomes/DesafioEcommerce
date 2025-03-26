using DesafioEcommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioEcommerce.Data
{
	// Add-Migration CreateDatabase -Context EcommerceDbContext
	// Update-Database -Context EcommerceDbContext  

	public class EcommerceDbContext : DbContext
	{
		public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options) { }

		public DbSet<Pedido> Pedidos { get; set; }
		public DbSet<Cliente> Clientes { get; set; }
		public DbSet<ItemPedido> ItensPedidos { get; set; }
	}
}
