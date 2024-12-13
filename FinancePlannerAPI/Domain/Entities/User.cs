using System.ComponentModel.DataAnnotations.Schema;

namespace FinancePlannerAPI.Domain.Entities
{
    public partial class User
    {
        public Guid Id { get; set; }

        public string Login { get; set; } = null!;

        public string PassHash { get; set; } = null!;

        public string Email { get; set; } = null!;

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

        public virtual ICollection<FinancialGoal> FinancialGoals { get; set; } = new List<FinancialGoal>();

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
