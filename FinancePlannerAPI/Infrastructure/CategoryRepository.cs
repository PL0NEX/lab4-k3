using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancePlannerApi.Infrastructure
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FinancePlannerContext _context;

        public CategoryRepository(FinancePlannerContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllByUserIdAsync(Guid userId)
        {
            return await _context.Categories.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}