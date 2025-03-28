using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinanceTracker.Domain.Models
{
    [Table("expense")]
    public class Expense  : BaseModel
    {
        public long Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("date")]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        [Column("description")]
        public string Description { get; set; }
        [Column("payment_method_id")]
        public int? PaymentMethodId { get; set; }

        [Column("created_by")]
        public Guid? CreatedBy { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("updated_by")]
        public Guid? UpdatedBy { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("isDeleted")]
        public bool IsDeleted { get; set; }

        public virtual ICollection<ExpenseItem> ExpenseItems { get; set; }
            = new List<ExpenseItem>();
    }

    public class ExpenseDto
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int? PaymentMethodId { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public List<ExpenseItemDto> ExpenseItems { get; set; }
    = new List<ExpenseItemDto>();
    }

    public class CreateExpenseDto
    {
        public Guid UserId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public int? PaymentMethodId { get; set; }
        public Guid? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class UpdateExpenseDto
    {
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public int? PaymentMethodId { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}


