using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Entities
{
    public class Pedido : BaseEntity 
    {
        public decimal Subtotal { get; private set; }
        public decimal PercentualDesconto { get; private set; }
        public decimal ValorDesconto { get; private set; }
        public decimal Total { get; private set; }
        public ICollection<ItemPedido> Itens { get; private set; } = new List<ItemPedido>();

        protected Pedido() { }

        public Pedido(decimal subtotal, decimal percentualDesconto)
        {
            Subtotal = subtotal;
            PercentualDesconto = percentualDesconto;
            CalcularDesconto();
        }

        private void CalcularDesconto()
        {
            ValorDesconto = Subtotal * (PercentualDesconto / 100m);
            Total = Subtotal - ValorDesconto;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AdicionarItem(ItemPedido item)
        {
            Itens.Add(item);
        }

        public void AtualizarValores(decimal subtotal, decimal percentualDesconto)
        {
            Subtotal = subtotal;
            PercentualDesconto = percentualDesconto;
            CalcularDesconto();
        }
    }
}