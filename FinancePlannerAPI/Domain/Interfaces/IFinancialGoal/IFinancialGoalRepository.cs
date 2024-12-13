using FinancePlannerAPI.Domain.Entities;

namespace FinancePlannerAPI.Domain.Interfaces
{
    public interface IFinancialGoalRepository
    {
        Task<FinancialGoal?> GetByIdAsync(Guid id);
        Task<IEnumerable<FinancialGoal>> GetAllByUserIdAsync(Guid userId);
        Task AddAsync(FinancialGoal financialGoal);
        Task UpdateAsync(FinancialGoal financialGoal);
        Task DeleteAsync(Guid id);
    }
}