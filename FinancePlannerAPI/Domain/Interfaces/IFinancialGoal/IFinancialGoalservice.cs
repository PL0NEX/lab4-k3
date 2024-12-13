using FinancePlannerAPI.Domain.Entities;

namespace FinancePlannerAPI.Domain.Interfaces
{
    public interface IFinancialGoalService
    {
        Task<FinancialGoal?> GetFinancialGoalByIdAsync(Guid id);
        Task<IEnumerable<FinancialGoal>> GetAllFinancialGoalsByUserIdAsync(Guid userId);
        Task AddFinancialGoalAsync(Guid userId, string name, decimal targetAmount, DateTime startDate, DateTime endDate);
        Task UpdateFinancialGoalAsync(Guid id, string name, decimal targetAmount, decimal currentAmount, DateTime endDate);
        Task DeleteFinancialGoalAsync(Guid id);
        Task UpdateProgressAsync(Guid id, decimal amount); 
    }
}