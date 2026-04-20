using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<Pedido>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(Pedido pedido, CancellationToken ct = default);
        void Update(Pedido pedido);
        void Delete(Pedido pedido);
        Task<bool> SaveChangesAsync(CancellationToken ct = default);
    }
}