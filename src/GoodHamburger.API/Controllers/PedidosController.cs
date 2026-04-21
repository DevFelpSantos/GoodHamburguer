namespace GoodHamburger.API.Controllers
{
    using GoodHamburger.API.Abstractions;
    using GoodHamburger.API.Responses;
    using GoodHamburger.Application.DTOs;
    using GoodHamburger.Application.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class PedidosController : ApiController
    {
        private readonly IPedidoService _service;

        public PedidosController(IPedidoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PedidoResponseDto>>> Create(
        [FromBody] CriarPedidoRequestDto dto,
        CancellationToken ct)
        {
            var result = await _service.CriarPedidoAsync(dto, ct);

            return CreatedAtAction(nameof(GetById), new { id = result!.Id },
                new ApiResponse<PedidoResponseDto>
                {
                    Success = true,
                    Message = "Pedido criado com sucesso",
                    Data = result
                });
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<PedidoResponseDto>>>> Get(CancellationToken ct)
        {
            var result = await _service.ListarPedidosAsync(ct);

            return Ok(new ApiResponse<List<PedidoResponseDto>>
            {
                Success = true,
                Message = "Pedidos listados com sucesso",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PedidoResponseDto>>> GetById(int id, CancellationToken ct)
        {
            var result = await _service.ObterPedidoPorIdAsync(id, ct);

            if (result == null)
                throw new KeyNotFoundException($"Pedido com ID {id} não encontrado.");

            return Ok(new ApiResponse<PedidoResponseDto>
            {
                Success = true,
                Message = "Pedido encontrado",
                Data = result
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PedidoResponseDto>>> Update(
        int id,
        [FromBody] CriarPedidoRequestDto dto,
        CancellationToken ct)
        {
            var result = await _service.AtualizarPedidoAsync(id, dto, ct);

            return Ok(new ApiResponse<PedidoResponseDto>
            {
                Success = true,
                Message = "Pedido atualizado com sucesso",
                Data = result!
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id, CancellationToken ct)
        {
            var result = await _service.DeletarPedidoAsync(id, ct);

            if (!result)
                throw new KeyNotFoundException($"Pedido com ID {id} não encontrado.");

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Pedido removido com sucesso",
                Data = null!
            });
        }
    }
}