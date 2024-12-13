using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancePlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound("Транзакция не найдена");
            }

            return Ok(transaction);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllTransactionsByUserId(Guid userId)
        {
            var transactions = await _transactionService.GetAllTransactionsByUserIdAsync(userId);
            return Ok(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            await _transactionService.AddTransactionAsync(transaction.UserId, transaction.Type, transaction.Amount, transaction.CategoryId, transaction.Description);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] Transaction transaction)
        {
            try
            {
                await _transactionService.UpdateTransactionAsync(id, transaction.Type, transaction.Amount, transaction.CategoryId, transaction.Description);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            try
            {
                await _transactionService.DeleteTransactionAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}/total/{type}")]
        public async Task<IActionResult> GetTotalAmountByType(Guid userId, string type)
        {
            var total = await _transactionService.GetTotalAmountByUserIdAsync(userId, type);
            return Ok(new { Total = total });
        }
    }
}