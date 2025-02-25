using FinanceTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Domain.Models
{
    public class ExpenseItem
    {
        public int Id { get; set; }
        public int ExpenseId { get; set; }
        public int CategoryId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Computed property for individual item total
        [NotMapped] // Exclude from database
        public decimal Total => Price * Quantity;

        public virtual Expense Expense { get; set; }
        public virtual Category Category { get; set; }
    }
}
