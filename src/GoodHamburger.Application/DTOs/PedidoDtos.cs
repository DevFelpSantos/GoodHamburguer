namespace GoodHamburger.Application.DTOs
{
    public record CriarPedidoRequestDto(
        List<int> IdsItens
    );

    public record ItemPedidoResponseDto(
        int MenuItemId,
        string NomeItem,
        decimal PrecoUnitario,
        string Tipo
    );

    public record PedidoResponseDto(
        int Id,
        List<ItemPedidoResponseDto> Itens,
        decimal Subtotal,
        decimal PercentualDesconto,
        decimal ValorDesconto,
        decimal Total,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}