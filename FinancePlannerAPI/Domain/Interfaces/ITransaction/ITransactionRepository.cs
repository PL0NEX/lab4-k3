using FinancePlannerAPI.Domain.Entities;

namespace FinancePlannerAPI.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(Guid id);
        Task<IEnumerable<Transaction>> GetAllByUserIdAsync(Guid userId);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Guid id);
    }
}