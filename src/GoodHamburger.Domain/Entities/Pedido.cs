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
            ValidarSubtotal(subtotal);
            ValidarPercentualDesconto(percentualDesconto);

            Subtotal = subtotal;
            PercentualDesconto = percentualDesconto;
            CalcularDesconto();
        }
        public static Pedido CriarDoItens(List<ItemPedido> itens, decimal percentualDesconto = 0)
        {
            if (itens == null || !itens.Any())
                throw new ArgumentException("Pedido deve conter pelo menos um item.");

            var subtotal = CalcularSubtotal(itens);

            var pedido = new Pedido(subtotal, percentualDesconto);

            foreach (var item in itens)
            {
                pedido.AdicionarItem(item);
            }

            return pedido;
        }



        public static decimal CalcularSubtotal(List<ItemPedido> itens)
        {
            if (itens == null || !itens.Any())
                return 0m;

            return itens.Sum(i => i.PrecoUnitario);
        }

        private void CalcularDesconto()
        {
            ValorDesconto = Subtotal * (PercentualDesconto / 100m);
            Total = Subtotal - ValorDesconto;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AdicionarItem(ItemPedido item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
                
            Itens.Add(item);
        }

        public void AtualizarValores(decimal subtotal, decimal percentualDesconto)
        {
            ValidarSubtotal(subtotal);
            ValidarPercentualDesconto(percentualDesconto);

            Subtotal = subtotal;
            PercentualDesconto = percentualDesconto;
            CalcularDesconto();
        }

        private static void ValidarSubtotal(decimal subtotal)
        {
            if (subtotal < 0)
                throw new ArgumentException("Subtotal não pode ser negativo.");
        }

        private static void ValidarPercentualDesconto(decimal percentualDesconto)
        {
            if (percentualDesconto < 0 || percentualDesconto > 100)
                throw new ArgumentException("Percentual de desconto deve estar entre 0 e 100.");
        }
    }
}