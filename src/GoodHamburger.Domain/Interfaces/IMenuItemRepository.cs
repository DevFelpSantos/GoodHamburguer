using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Interfaces
{
    public interface IMenuItemRepository
    {
        Task<MenuItem?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<MenuItem>> GetAllAsync(CancellationToken ct = default);
        Task<MenuItem?> GetByNameAsync(string nome, CancellationToken ct = default);
        Task AddAsync(MenuItem menuItem, CancellationToken ct = default);
        void Update(MenuItem menuItem);
        void Delete(MenuItem menuItem);
        Task<bool> SaveChangesAsync(CancellationToken ct = default);
    }
}