namespace GoodHamburger.Application.DTOs
{
    public record MenuItemResponseDto(
        int Id,
        string Nome,
        decimal Preco,
        string Tipo,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}