using System.ComponentModel.DataAnnotations.Schema;

namespace FinancePlannerAPI.Domain.Entities
{
    public partial class Category
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; } = null!;

        public virtual User User { get; set; } = null!;

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
