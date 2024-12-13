using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancePlannerApi.Infrastructure
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly FinancePlannerContext _context;

        public TransactionRepository(FinancePlannerContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetByIdAsync(Guid id)
        {
            return await _context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.Transactions.Include(t => t.Category).Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var transaction = await GetByIdAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}