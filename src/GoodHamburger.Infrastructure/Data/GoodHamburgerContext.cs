namespace GoodHamburger.Infrastructure.Data
{
    using GoodHamburger.Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class GoodHamburgerContext : DbContext
    {
        public GoodHamburgerContext(DbContextOptions<GoodHamburgerContext> options)
            : base(options) { }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Preco).HasPrecision(10, 2);
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired(false);
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Subtotal).HasPrecision(10, 2);
                entity.Property(e => e.PercentualDesconto).HasPrecision(5, 2);
                entity.Property(e => e.ValorDesconto).HasPrecision(10, 2);
                entity.Property(e => e.Total).HasPrecision(10, 2);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired(false);

                entity.HasMany(e => e.Itens)
                    .WithOne(i => i.Pedido)
                    .HasForeignKey(i => i.PedidoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ItemPedido>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NomeItem).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PrecoUnitario).HasPrecision(10, 2);
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired(false);

                entity.HasOne(e => e.MenuItem)
                    .WithMany()
                    .HasForeignKey(e => e.MenuItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            SeedCardapio(modelBuilder);
        }

        private static void SeedCardapio(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem(1, "X Burger", 5.00m, "Sanduiche"),
                new MenuItem(2, "X Egg", 4.50m, "Sanduiche"),
                new MenuItem(3, "X Bacon", 7.00m, "Sanduiche"),
                new MenuItem(4, "Batata Frita", 2.00m, "Acompanhamento"),
                new MenuItem(5, "Refrigerante", 2.50m, "Bebida")
            );
        }
    }
}