using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinanceTracker.Domain.Models
{
    [Table("expense")]
    public class Expense
    {
        public long Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("date")]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }
        public int? PaymentMethodId { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<ExpenseItem> ExpenseItems { get; set; }
            = new List<ExpenseItem>();
    }

}
