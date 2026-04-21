namespace GoodHamburger.Tests.Domain
{
    using Xunit;
    using GoodHamburger.Domain.Entities;
    public class PedidoTests
    {
        [Fact]
        public void Constructor_ComSubtotalEDesconto_CriaCalculosCorretamente()
        {
            decimal subtotal = 9.50m;
            decimal percentualDesconto = 20m;

            var pedido = new Pedido(subtotal, percentualDesconto);

            Assert.Equal(subtotal, pedido.Subtotal);
            Assert.Equal(percentualDesconto, pedido.PercentualDesconto);
            Assert.Equal(1.90m, pedido.ValorDesconto);
            Assert.Equal(7.60m, pedido.Total);
        }

        [Theory]
        [InlineData(10.00, 0, 0.00, 10.00)]
        [InlineData(10.00, 10, 1.00, 9.00)]
        [InlineData(10.00, 15, 1.50, 8.50)]
        [InlineData(10.00, 20, 2.00, 8.00)]
        public void Constructor_CalculaDescontoCorretamente(
            decimal subtotal,
            decimal percentual,
            decimal descontoEsperado,
            decimal totalEsperado)
        {
            var pedido = new Pedido(subtotal, percentual);

            Assert.Equal(descontoEsperado, pedido.ValorDesconto);
            Assert.Equal(totalEsperado, pedido.Total);
        }

        [Fact]
        public void AdicionarItem_AdicionaAoColecao()
        {
            var pedido = new Pedido(10.00m, 0);
            var menuItem = new MenuItem(1, "X Burger", 5.00m, "Sanduiche");
            var itemPedido = new ItemPedido(menuItem.Id, menuItem);

            Assert.Empty(pedido.Itens);

            pedido.AdicionarItem(itemPedido);

            Assert.Single(pedido.Itens);
            Assert.Contains(itemPedido, pedido.Itens);
        }

        [Fact]
        public void AtualizarValores_RecalculaDescontoETotal()
        {
            var pedido = new Pedido(10.00m, 0);

            pedido.AtualizarValores(20.00m, 10);

            Assert.Equal(20.00m, pedido.Subtotal);
            Assert.Equal(10m, pedido.PercentualDesconto);
            Assert.Equal(2.00m, pedido.ValorDesconto);
            Assert.Equal(18.00m, pedido.Total);
        }
    }
}