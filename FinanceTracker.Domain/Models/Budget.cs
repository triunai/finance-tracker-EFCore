using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Domain.Models
{
    [Table("budget")]
    public class Budget
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public Period Period { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<BudgetCategory> BudgetCategories { get; set; }
            = new List<BudgetCategory>();
    }
}
