using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;

namespace FinancePlannerAPI.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesByUserIdAsync(Guid userId)
        {
            return await _categoryRepository.GetAllByUserIdAsync(userId);
        }

        public async Task CreateCategoryAsync(Guid userId, string name)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = name
            };

            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(Guid id, string name)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Категория не найдена");
            }

            category.Name = name;
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Категория не найдена");
            }

            await _categoryRepository.DeleteAsync(id);
        }
    }
}
