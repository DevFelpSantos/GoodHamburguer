using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Services.Interfaces;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.Services
{
    public class CardapioService : ICardapioService
    {
        private readonly IMenuItemRepository _repository;

        public CardapioService(IMenuItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MenuItemResponseDto>> ObterCardapioAsync(CancellationToken ct = default)
        {
            var items = await _repository.GetAllAsync(ct);
            return items.Select(i => new MenuItemResponseDto(
                i.Id,
                i.Nome,
                i.Preco,
                i.Tipo,
                i.CreatedAt,
                i.UpdatedAt
            )).ToList();
        }
    }
}