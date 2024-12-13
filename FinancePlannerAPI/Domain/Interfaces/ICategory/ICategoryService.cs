using FinancePlannerAPI.Domain.Entities;

namespace FinancePlannerAPI.Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<Category?> GetCategoryByIdAsync(Guid id);
        Task<IEnumerable<Category>> GetAllCategoriesByUserIdAsync(Guid userId);
        Task CreateCategoryAsync(Guid userId, string name);
        Task UpdateCategoryAsync(Guid id, string name);
        Task DeleteCategoryAsync(Guid id);
    }
}
