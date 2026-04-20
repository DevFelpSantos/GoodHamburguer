namespace GoodHamburger.Application.Services.Interfaces
{
    using GoodHamburger.Application.DTOs;
    public interface ICardapioService
    {
        Task<List<MenuItemResponseDto>> ObterCardapioAsync(CancellationToken ct = default);

    }
}