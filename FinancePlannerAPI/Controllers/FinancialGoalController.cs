using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancePlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancialGoalsController : ControllerBase
    {
        private readonly IFinancialGoalService _financialGoalService;

        public FinancialGoalsController(IFinancialGoalService financialGoalService)
        {
            _financialGoalService = financialGoalService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFinancialGoalById(Guid id)
        {
            var financialGoal = await _financialGoalService.GetFinancialGoalByIdAsync(id);
            if (financialGoal == null)
            {
                return NotFound("Финансовая цель не найдена");
            }

            return Ok(financialGoal);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllFinancialGoalsByUserId(Guid userId)
        {
            var financialGoals = await _financialGoalService.GetAllFinancialGoalsByUserIdAsync(userId);
            return Ok(financialGoals);
        }

        [HttpPost]
        public async Task<IActionResult> AddFinancialGoal([FromBody] FinancialGoal financialGoal)
        {
            await _financialGoalService.AddFinancialGoalAsync(financialGoal.UserId, financialGoal.Name, financialGoal.TargetAmount, financialGoal.StartDate, financialGoal.EndDate);
            return CreatedAtAction(nameof(GetFinancialGoalById), new { id = financialGoal.Id }, financialGoal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFinancialGoal(Guid id, [FromBody] FinancialGoal financialGoal)
        {
            try
            {
                await _financialGoalService.UpdateFinancialGoalAsync(id, financialGoal.Name, financialGoal.TargetAmount, financialGoal.CurrentAmount, financialGoal.EndDate);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}