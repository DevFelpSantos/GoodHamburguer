namespace GoodHamburger.Infrastructure.Repositories
{
    using GoodHamburger.Domain.Entities;
    using GoodHamburger.Domain.Interfaces;
    using GoodHamburger.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    public class PedidoRepository : IPedidoRepository
    {
        private readonly GoodHamburgerContext _context;

        public PedidoRepository(GoodHamburgerContext context)
        {
            _context = context;
        }

        public async Task<Pedido?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<List<Pedido>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .ToListAsync(ct);
        }

        public async Task AddAsync(Pedido pedido, CancellationToken ct = default)
        {
            await _context.Pedidos.AddAsync(pedido, ct);
        }

        public void Update(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
        }

        public void Delete(Pedido pedido)
        {
            _context.Pedidos.Remove(pedido);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct) > 0;
        }
    }
}