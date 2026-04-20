namespace GoodHamburger.API.Controllers
{
    using GoodHamburger.API.Abstractions;
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
        [ProducesResponseType(typeof(PedidoResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PedidoResponseDto>> Create(
            [FromBody] CriarPedidoRequestDto dto,
            CancellationToken ct)
        {
            try
            {
                var result = await _service.CriarPedidoAsync(dto, ct);
                return CreatedAtAction(nameof(GetById), new { id = result!.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<PedidoResponseDto>>> Get(CancellationToken ct)
        {
            try
            {
                var result = await _service.ListarPedidosAsync(ct);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PedidoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PedidoResponseDto>> GetById(int id, CancellationToken ct)
        {
            try
            {
                var result = await _service.ObterPedidoPorIdAsync(id, ct);
                if (result == null)
                    return NotFound(new { mensagem = $"Pedido com ID {id} não encontrado." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PedidoResponseDto>> Update(
            int id,
            [FromBody] CriarPedidoRequestDto dto,
            CancellationToken ct)
        {
            try
            {
                var result = await _service.AtualizarPedidoAsync(id, dto, ct);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            try
            {
                var result = await _service.DeletarPedidoAsync(id, ct);
                if (!result)
                    return NotFound(new { mensagem = $"Pedido com ID {id} não encontrado." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}