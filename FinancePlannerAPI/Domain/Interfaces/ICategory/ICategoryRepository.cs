using FinancePlannerAPI.Domain.Entities;

namespace FinancePlannerAPI.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id);
        Task<IEnumerable<Category>> GetAllByUserIdAsync(Guid userId);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Guid id);
    }
}