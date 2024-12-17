using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancePlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    _logger.LogWarning("Транзакция с id {Id} не найдена", id);
                    return NotFound("Транзакция не найдена");
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении транзакции с id {Id}", id);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllTransactionsByUserId(Guid userId)
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionsByUserIdAsync(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении транзакций пользователя с id {UserId}", userId);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            try
            {
                if (transaction == null)
                {
                    return BadRequest("Данные транзакции не могут быть пустыми");
                }

                await _transactionService.AddTransactionAsync(
                    transaction.UserId, transaction.Type, transaction.Amount, transaction.CategoryId, transaction.Description
                );
                return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Ошибка при добавлении транзакции. Возможно, транзакция уже существует.");
                return Conflict("Транзакция с таким id уже существует");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении транзакции");
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] Transaction transaction)
        {
            try
            {
                if (transaction == null)
                {
                    return BadRequest("Данные транзакции не могут быть пустыми");
                }

                await _transactionService.UpdateTransactionAsync(
                    id, transaction.Type, transaction.Amount, transaction.CategoryId, transaction.Description
                );
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Транзакция с id {Id} не найдена", id);
                return NotFound("Транзакция не найдена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении транзакции с id {Id}", id);
                return StatusCode(500, "Произошла неизвестная ошибка");
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
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Транзакция с id {Id} не найдена", id);
                return NotFound("Транзакция не найдена");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении транзакции с id {Id}", id);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }

        [HttpGet("user/{userId}/total/{type}")]
        public async Task<IActionResult> GetTotalAmountByType(Guid userId, string type)
        {
            try
            {
                var total = await _transactionService.GetTotalAmountByUserIdAsync(userId, type);
                return Ok(new { Total = total });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении общей суммы для пользователя {UserId} и типа {Type}", userId, type);
                return StatusCode(500, "Произошла неизвестная ошибка");
            }
        }
    }
}
