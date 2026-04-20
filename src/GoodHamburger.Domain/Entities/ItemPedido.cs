namespace GoodHamburger.Domain.Entities
{
    public class ItemPedido : BaseEntity
    {
        public int PedidoId { get; private set; }
        public Pedido Pedido { get; private set; }

        public int MenuItemId { get; private set; }
        public MenuItem MenuItem { get; private set; }

        public string NomeItem { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public string Tipo { get; private set; }

        protected ItemPedido() { }

        public ItemPedido(int menuItemId, MenuItem menuItem)
        {
            MenuItemId = menuItemId;
            MenuItem = menuItem ?? throw new ArgumentNullException(nameof(menuItem));
            NomeItem = menuItem.Nome;
            PrecoUnitario = menuItem.Preco;
            Tipo = menuItem.Tipo;
        }
    }
}