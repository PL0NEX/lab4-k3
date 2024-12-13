using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;

namespace FinancePlannerAPI.Domain.Services
{
    public class FinancialGoalService : IFinancialGoalService
    {
        private readonly IFinancialGoalRepository _financialGoalRepository;

        public FinancialGoalService(IFinancialGoalRepository financialGoalRepository)
        {
            _financialGoalRepository = financialGoalRepository;
        }

        public async Task<FinancialGoal?> GetFinancialGoalByIdAsync(Guid id)
        {
            return await _financialGoalRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<FinancialGoal>> GetAllFinancialGoalsByUserIdAsync(Guid userId)
        {
            return await _financialGoalRepository.GetAllByUserIdAsync(userId);
        }

        public async Task AddFinancialGoalAsync(Guid userId, string name, decimal targetAmount, DateTime startDate, DateTime endDate)
        {
            var financialGoal = new FinancialGoal
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = name,
                TargetAmount = targetAmount,
                CurrentAmount = 0,
                StartDate = startDate,
                EndDate = endDate
            };

            await _financialGoalRepository.AddAsync(financialGoal);
        }

        public async Task UpdateFinancialGoalAsync(Guid id, string name, decimal targetAmount, decimal currentAmount, DateTime endDate)
        {
            var financialGoal = await _financialGoalRepository.GetByIdAsync(id);
            if (financialGoal == null)
            {
                throw new Exception("Финансовая цель не найдена");
            }

            financialGoal.Name = name;
            financialGoal.TargetAmount = targetAmount;
            financialGoal.CurrentAmount = currentAmount;
            financialGoal.EndDate = endDate;

            await _financialGoalRepository.UpdateAsync(financialGoal);
        }

        public async Task DeleteFinancialGoalAsync(Guid id)
        {
            var financialGoal = await _financialGoalRepository.GetByIdAsync(id);
            if (financialGoal == null)
            {
                throw new Exception("Финансовая цель не найдена");
            }

            await _financialGoalRepository.DeleteAsync(id);
        }

        public async Task UpdateProgressAsync(Guid id, decimal amount)
        {
            var financialGoal = await _financialGoalRepository.GetByIdAsync(id);
            if (financialGoal == null)
            {
                throw new Exception("Финансовая цель не найдена");
            }

            financialGoal.CurrentAmount += amount;

            if (financialGoal.CurrentAmount > financialGoal.TargetAmount)
            {
                financialGoal.CurrentAmount = financialGoal.TargetAmount;
            }

            await _financialGoalRepository.UpdateAsync(financialGoal);
        }
    }
}
