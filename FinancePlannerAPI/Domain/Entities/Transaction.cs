using System.ComponentModel.DataAnnotations.Schema;

namespace FinancePlannerAPI.Domain.Entities
{
    public partial class Transaction
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Type { get; set; } = null!;

        public Guid? CategoryId { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string? Description { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual Category? Category { get; set; }
    }
}
