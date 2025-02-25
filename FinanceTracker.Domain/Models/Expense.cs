using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinanceTracker.Domain.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        
        //todo: ask gpt WTFFF
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? UpdatedByUserId { get; set; }

        public virtual ICollection<ExpenseItem> ExpenseItems { get; set; } = new HashSet<ExpenseItem>();
    }
}
