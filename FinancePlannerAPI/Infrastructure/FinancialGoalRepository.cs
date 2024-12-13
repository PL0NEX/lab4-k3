using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancePlannerApi.Infrastructure
{
    public class FinancialGoalRepository : IFinancialGoalRepository
    {
        private readonly FinancePlannerContext _context;

        public FinancialGoalRepository(FinancePlannerContext context)
        {
            _context = context;
        }

        public async Task<FinancialGoal?> GetByIdAsync(Guid id)
        {
            return await _context.FinancialGoals.FirstOrDefaultAsync(fg => fg.Id == id);
        }

        public async Task<IEnumerable<FinancialGoal>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.FinancialGoals.Where(fg => fg.UserId == userId).ToListAsync();
        }

        public async Task AddAsync(FinancialGoal financialGoal)
        {
            await _context.FinancialGoals.AddAsync(financialGoal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FinancialGoal financialGoal)
        {
            _context.FinancialGoals.Update(financialGoal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var financialGoal = await GetByIdAsync(id);
            if (financialGoal != null)
            {
                _context.FinancialGoals.Remove(financialGoal);
                await _context.SaveChangesAsync();
            }
        }
    }
}