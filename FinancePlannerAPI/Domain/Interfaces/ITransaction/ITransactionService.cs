using FinancePlannerAPI.Domain.Entities;

namespace FinancePlannerAPI.Domain.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction?> GetTransactionByIdAsync(Guid id);
        Task<IEnumerable<Transaction>> GetAllTransactionsByUserIdAsync(Guid userId);
        Task AddTransactionAsync(Guid userId, string type, decimal amount, Guid? categoryId, string? description);
        Task UpdateTransactionAsync(Guid id, string type, decimal amount, Guid? categoryId, string? description);
        Task DeleteTransactionAsync(Guid id);
        Task<decimal> GetTotalAmountByUserIdAsync(Guid userId, string type); 
    }
}