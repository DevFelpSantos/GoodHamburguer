namespace GoodHamburger.API.Controllers
{
    using GoodHamburger.API.Abstractions;
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
        public async Task<ActionResult<List<MenuItemResponseDto>>> Get(CancellationToken ct)
        {
            try
            {
                var items = await _service.ObterCardapioAsync(ct);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}

