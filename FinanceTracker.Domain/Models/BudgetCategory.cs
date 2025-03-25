using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Domain.Models
{
    [Table("budget_category")]
    public class BudgetCategory
    {
        [Key, Column(Order = 0)]
        public long BudgetId { get; set; }
        [Key, Column(Order = 1)]
        public long CategoryId { get; set; }

        public decimal? AlertThreshold { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Budget Budget { get; set; }
        public virtual ExpenseCategory ExpenseCategory { get; set; }
    }
}
