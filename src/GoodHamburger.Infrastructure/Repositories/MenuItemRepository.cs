
namespace GoodHamburger.Infrastructure.Repositories
{
    using GoodHamburger.Domain.Entities;
    using GoodHamburger.Domain.Interfaces;
    using GoodHamburger.Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly GoodHamburgerContext _context;

        public MenuItemRepository(GoodHamburgerContext context)
        {
            _context = context;
        }

        public async Task<MenuItem?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == id, ct);
        }

        public async Task<List<MenuItem>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.MenuItems.ToListAsync(ct);
        }

        public async Task<MenuItem?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            return await _context.MenuItems.FirstOrDefaultAsync(m => m.Nome == name, ct);
        }

        public async Task AddAsync(MenuItem item, CancellationToken ct = default)
        {
            await _context.MenuItems.AddAsync(item, ct);
        }

        public void Update(MenuItem item)
        {
            _context.MenuItems.Update(item);
        }

        public void Delete(MenuItem item)
        {
            _context.MenuItems.Remove(item);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct) > 0;
        }
    }

}