using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancePlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound("Категория не найдена");
            }

            return Ok(category);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllCategoriesByUserId(Guid userId)
        {
            var categories = await _categoryService.GetAllCategoriesByUserIdAsync(userId);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            await _categoryService.CreateCategoryAsync(category.UserId, category.Name);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] Category category)
        {
            try
            {
                await _categoryService.UpdateCategoryAsync(id, category.Name);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}