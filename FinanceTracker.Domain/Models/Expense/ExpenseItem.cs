using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Domain.Models
{
    [Table("expense_item")]
    public class ExpenseItem
    {
        public long Id { get; set; }
        public long ExpenseId { get; set; }
        public long CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public decimal Total => Amount;

        public virtual expense Expense { get; set; }
        public virtual ExpenseCategory Category { get; set; }
    }
    /// <summary>
    /// Read/View DTO for displaying an ExpenseItem
    /// </summary>
    public class ExpenseItemDto
    {
        public long Id { get; set; }
        public long ExpenseId { get; set; }
        public long CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // If you want to expose the computed "Total",
        // it simply mirrors Amount in this case. 
        public decimal Total => Amount;
    }

    /// <summary>
    /// DTO for creating a new ExpenseItem
    /// </summary>
    public class CreateExpenseItemDto
    {
        // Typically set on create
        public long ExpenseId { get; set; }
        public long CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        // Optional auditing flags if you let clients specify them
        public Guid? CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// DTO for updating an existing ExpenseItem
    /// </summary>
    public class UpdateExpenseItemDto
    {
        // Fields marked as nullable so you can partially update
        public long? CategoryId { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }

        // Typically, ExpenseId is not changed once the item is associated,
        // so it's omitted here or made read-only if your business logic allows.
    }

}
