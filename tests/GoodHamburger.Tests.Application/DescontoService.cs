namespace GoodHamburger.Tests.Application
{
    using Xunit;
    using GoodHamburger.Application.Services;
    using GoodHamburger.Domain.Entities;
    public class DescontoServiceTests
    {
        private readonly DescontoService _service;

        public DescontoServiceTests()
        {
            _service = new DescontoService();
        }

        [Fact]
        public void CalcularPercentualDesconto_ComSanduicheEBatataERefrigerante_Retorna20()
        {
            var menuItem1 = new MenuItem(1, "X Burger", 5.00m, "Sanduiche");
            var menuItem4 = new MenuItem(4, "Batata Frita", 2.00m, "Acompanhamento");
            var menuItem5 = new MenuItem(5, "Refrigerante", 2.50m, "Bebida");

            var itens = new List<ItemPedido>
            {
                new ItemPedido(menuItem1.Id, menuItem1),
                new ItemPedido(menuItem4.Id, menuItem4),
                new ItemPedido(menuItem5.Id, menuItem5)
            };

            var resultado = _service.CalcularPercentualDesconto(itens);

            Assert.Equal(20m, resultado);
        }

        [Fact]
        public void CalcularPercentualDesconto_ComSanduicheERefrigerante_Retorna15()
        {
            var menuItem1 = new MenuItem(1, "X Burger", 5.00m, "Sanduiche");
            var menuItem5 = new MenuItem(5, "Refrigerante", 2.50m, "Bebida");

            var itens = new List<ItemPedido>
            {
                new ItemPedido(menuItem1.Id, menuItem1),
                new ItemPedido(menuItem5.Id, menuItem5)
            };

            var resultado = _service.CalcularPercentualDesconto(itens);

            Assert.Equal(15m, resultado);
        }

        [Fact]
        public void CalcularPercentualDesconto_ComSanduicheEBatata_Retorna10()
        {
            var menuItem1 = new MenuItem(1, "X Burger", 5.00m, "Sanduiche");
            var menuItem4 = new MenuItem(4, "Batata Frita", 2.00m, "Acompanhamento");

            var itens = new List<ItemPedido>
            {
                new ItemPedido(menuItem1.Id, menuItem1),
                new ItemPedido(menuItem4.Id, menuItem4)
            };

            var resultado = _service.CalcularPercentualDesconto(itens);

            Assert.Equal(10m, resultado);
        }

        [Fact]
        public void CalcularPercentualDesconto_ComApenasUmSanduiche_Retorna0()
        {
            var menuItem1 = new MenuItem(1, "X Burger", 5.00m, "Sanduiche");

            var itens = new List<ItemPedido>
            {
                new ItemPedido(menuItem1.Id, menuItem1)
            };

            var resultado = _service.CalcularPercentualDesconto(itens);

            Assert.Equal(0m, resultado);
        }

        [Fact]
        public void CalcularValorDesconto_ComSubtotal9_5E20Porcento_Retorna1_9()
        {
            decimal subtotal = 9.50m;
            decimal percentual = 20m;

            var resultado = _service.CalcularValorDesconto(subtotal, percentual);

            Assert.Equal(1.90m, resultado);
        }
    }
}