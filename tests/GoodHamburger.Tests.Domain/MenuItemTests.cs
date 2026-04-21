namespace GoodHamburger.Tests.Domain
{
    using GoodHamburger.Domain.Entities;
    using Xunit;
    public class MenuItemTests
    {
        [Fact]
        public void Constructor_comDadosValidos_CriarMenuItem()
        {
            int id = 1;
            string nome = "X Burger";
            decimal preco = 5.00m;
            string tipo = "Sanduiche";

            var menuItem = new MenuItem(id, nome, preco, tipo);

            Assert.Equal(id, menuItem.Id);
            Assert.Equal(nome, menuItem.Nome);
            Assert.Equal(preco, menuItem.Preco);
            Assert.Equal(tipo, menuItem.Tipo);
            Assert.True(menuItem.Ativo);
            Assert.NotNull(menuItem.CreatedAt);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(-0.01)]
        public void Constructor_ComPrecoinvalido_ThrowArgumentException(decimal preco)
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new MenuItem(1, "X Burger", preco, "Sanduiche")
            );

            Assert.Contains("Preco", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Atualizar_ComDadosValidos_AtualizaPropriedades()
        {
            var menuItem = new MenuItem(1, "X Burger", 5.00m, "Sanduiche");
            string novoNome = "X Bacon";
            decimal novoPreco = 7.00m;
            string novoTipo = "Sanduiche Premium";

            menuItem.Atualizar(novoNome, novoPreco, novoTipo);

            Assert.Equal(novoNome, menuItem.Nome);
            Assert.Equal(novoPreco, menuItem.Preco);
            Assert.Equal(novoTipo, menuItem.Tipo);
            Assert.NotNull(menuItem.UpdatedAt);
        }

        [Fact]
        public void Desativar_MarkaAtivoComoFalse()
        {
            var menuItem = new MenuItem(1, "X Burger", 5.00m, "Sanduiche");
            Assert.True(menuItem.Ativo);

            menuItem.Desativar();

            Assert.False(menuItem.Ativo);
            Assert.NotNull(menuItem.UpdatedAt);
        }
    }
}