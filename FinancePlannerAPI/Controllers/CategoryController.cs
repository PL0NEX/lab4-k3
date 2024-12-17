using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancePlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Категория с id {Id} не найдена", id);
                    return NotFound("Категория не найдена");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении категории с id {Id}", id);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllCategoriesByUserId(Guid userId)
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesByUserIdAsync(userId);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении категорий пользователя с id {UserId}", userId);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Данные категории не могут быть пустыми");
                }

                await _categoryService.CreateCategoryAsync(category.UserId, category.Name);
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Категория с таким id {Id} уже существует", category.Id);
                return Conflict("Категория с таким id уже существует");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании категории");
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] Category category)
        {
            try
            {
                if (category == null)
                {
                    return BadRequest("Данные категории не могут быть пустыми");
                }

                await _categoryService.UpdateCategoryAsync(id, category.Name);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Категория с id {Id} не найдена", id);
                return NotFound("Категория не найдена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении категории с id {Id}", id);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Категория с id {Id} не найдена", id);
                return NotFound("Категория не найдена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении категории с id {Id}", id);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }
    }
}
