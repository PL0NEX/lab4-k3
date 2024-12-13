using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;

namespace FinancePlannerAPI.Domain.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(Guid id)
        {
            return await _transactionRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsByUserIdAsync(Guid userId)
        {
            return await _transactionRepository.GetAllByUserIdAsync(userId);
        }

        public async Task AddTransactionAsync(Guid userId, string type, decimal amount, Guid? categoryId, string? description)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Type = type,
                Amount = amount,
                CategoryId = categoryId,
                TransactionDate = DateTime.UtcNow,
                Description = description
            };

            await _transactionRepository.AddAsync(transaction);
        }

        public async Task UpdateTransactionAsync(Guid id, string type, decimal amount, Guid? categoryId, string? description)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
            {
                throw new Exception("Транзакция не найдена");
            }

            transaction.Type = type;
            transaction.Amount = amount;
            transaction.CategoryId = categoryId;
            transaction.Description = description;

            await _transactionRepository.UpdateAsync(transaction);
        }

        public async Task DeleteTransactionAsync(Guid id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
            {
                throw new Exception("Транзакция не найдена");
            }

            await _transactionRepository.DeleteAsync(id);
        }

        public async Task<decimal> GetTotalAmountByUserIdAsync(Guid userId, string type)
        {
            var transactions = await _transactionRepository.GetAllByUserIdAsync(userId);
            return transactions.Where(t => t.Type == type).Sum(t => t.Amount);
        }
    }
}
