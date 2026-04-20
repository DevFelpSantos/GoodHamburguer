namespace GoodHamburger.Application.Services
{
    using GoodHamburger.Application.Services.Interfaces;
    using GoodHamburger.Domain.Entities;

    public class DescontoService : IDescontoService
    {
        public decimal CalcularPercentualDesconto(List<ItemPedido> itens)
        {
            bool temSanduiche = itens.Any(i => i.Tipo == "Sanduiche");
            bool temBatata = itens.Any(i => i.Tipo == "Acompanhamento");
            bool temRefrigerante = itens.Any(i => i.Tipo == "Bebida");

            if (temSanduiche && temBatata && temRefrigerante)
                return 20m;

            if (temSanduiche && temRefrigerante)
                return 15m;

            if (temSanduiche && temBatata)
                return 10m;

            return 0m;
        }

        public decimal CalcularValorDesconto(decimal subtotal, decimal percentualDesconto)
        {
            return subtotal * (percentualDesconto / 100m);
        }
    }
}