using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Domain.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public Period Period { get; set; } // Enum: Monthly/Weekly/Yearly
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Category Category { get; set; }
        public virtual ICollection<ExpenseItem> ExpenseItems { get; set; } // Optional: For direct queries
    }
}
