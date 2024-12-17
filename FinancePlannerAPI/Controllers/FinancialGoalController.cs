using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancePlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialGoalsController : ControllerBase
    {
        private readonly IFinancialGoalService _financialGoalService;
        private readonly ILogger<FinancialGoalsController> _logger;

        public FinancialGoalsController(IFinancialGoalService financialGoalService, ILogger<FinancialGoalsController> logger)
        {
            _financialGoalService = financialGoalService ?? throw new ArgumentNullException(nameof(financialGoalService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFinancialGoalById(Guid id)
        {
            try
            {
                var financialGoal = await _financialGoalService.GetFinancialGoalByIdAsync(id);
                if (financialGoal == null)
                {
                    _logger.LogWarning("Финансовая цель с id {Id} не найдена", id);
                    return NotFound("Финансовая цель не найдена");
                }

                return Ok(financialGoal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении финансовой цели с id {Id}", id);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllFinancialGoalsByUserId(Guid userId)
        {
            try
            {
                var financialGoals = await _financialGoalService.GetAllFinancialGoalsByUserIdAsync(userId);
                return Ok(financialGoals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении финансовых целей пользователя с id {UserId}", userId);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFinancialGoal([FromBody] FinancialGoal financialGoal)
        {
            try
            {
                if (financialGoal == null)
                {
                    return BadRequest("Данные финансовой цели не могут быть пустыми");
                }

                await _financialGoalService.AddFinancialGoalAsync(
                    financialGoal.UserId, financialGoal.Name, financialGoal.TargetAmount, financialGoal.StartDate, financialGoal.EndDate
                );
                return CreatedAtAction(nameof(GetFinancialGoalById), new { id = financialGoal.Id }, financialGoal);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Финансовая цель с id {Id} уже существует", financialGoal.Id);
                return Conflict("Финансовая цель с таким id уже существует");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении финансовой цели");
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFinancialGoal(Guid id, [FromBody] FinancialGoal financialGoal)
        {
            try
            {
                if (financialGoal == null)
                {
                    return BadRequest("Данные финансовой цели не могут быть пустыми");
                }

                await _financialGoalService.UpdateFinancialGoalAsync(
                    id, financialGoal.Name, financialGoal.TargetAmount, financialGoal.CurrentAmount, financialGoal.EndDate
                );
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Финансовая цель с id {Id} не найдена", id);
                return NotFound("Финансовая цель не найдена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении финансовой цели с id {Id}", id);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinancialGoal(Guid id)
        {
            try
            {
                await _financialGoalService.DeleteFinancialGoalAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Финансовая цель с id {Id} не найдена", id);
                return NotFound("Финансовая цель не найдена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении финансовой цели с id {Id}", id);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpPut("{id}/progress")]
        public async Task<IActionResult> UpdateProgress(Guid id, [FromBody] decimal amount)
        {
            try
            {
                await _financialGoalService.UpdateProgressAsync(id, amount);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Финансовая цель с id {Id} не найдена", id);
                return NotFound("Финансовая цель не найдена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении прогресса финансовой цели с id {Id}", id);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }
    }
}
