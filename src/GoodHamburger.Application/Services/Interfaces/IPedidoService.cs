namespace GoodHamburger.Application.Services.Interfaces
{
    using GoodHamburger.Application.DTOs;
    public interface IPedidoService
    {

        Task<PedidoResponseDto?> CriarPedidoAsync(CriarPedidoRequestDto request, CancellationToken ct = default);
        Task<List<PedidoResponseDto>> ListarPedidosAsync(CancellationToken ct = default);
        Task<PedidoResponseDto?> ObterPedidoPorIdAsync(int id, CancellationToken ct = default);
        Task<PedidoResponseDto?> AtualizarPedidoAsync(int id, CriarPedidoRequestDto request, CancellationToken ct = default);
        Task<bool> DeletarPedidoAsync(int id, CancellationToken ct = default);
    }
}
