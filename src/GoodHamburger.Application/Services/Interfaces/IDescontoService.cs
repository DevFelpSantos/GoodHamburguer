namespace GoodHamburger.Application.Services.Interfaces
{
    using GoodHamburger.Domain.Entities;
    public interface IDescontoService
    {
        decimal CalcularPercentualDesconto(List<ItemPedido> itens);
        decimal CalcularValorDesconto(decimal subtotal, decimal percentualDesconto);
    }
}
