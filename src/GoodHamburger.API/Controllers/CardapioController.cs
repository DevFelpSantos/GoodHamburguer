namespace GoodHamburger.API.Controllers
{
    using GoodHamburger.API.Abstractions;
    using GoodHamburger.API.Responses;
    using GoodHamburger.Application.DTOs;
    using GoodHamburger.Application.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class CardapioController : ApiController
    {
        private readonly ICardapioService _service;

        public CardapioController(ICardapioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<MenuItemResponseDto>>>> Get(CancellationToken ct)
        {
            var items = await _service.ObterCardapioAsync(ct);

            return Ok(new ApiResponse<List<MenuItemResponseDto>>
            {
                Success = true,
                Message = "Cardápio carregado com sucesso",
                Data = items
            });
        }
    }
}