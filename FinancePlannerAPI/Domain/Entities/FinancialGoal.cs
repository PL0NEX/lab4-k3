using System.ComponentModel.DataAnnotations.Schema;

namespace FinancePlannerAPI.Domain.Entities
{
    public partial class FinancialGoal
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; } = null!;

        public decimal TargetAmount { get; set; }

        public decimal CurrentAmount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
