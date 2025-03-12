using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Domain.Models
{
    public class PaymentMethod 
    {
        public int Id { get; set; }

        public string MethodName { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class PaymentMethodDto
    {
        public int Id { get; set; }
        public string MethodName { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class PaymentMethodCreateDto
    {
        public string MethodName { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } // Add this

        public bool IsDeleted { get; set; }
    }

    public class PaymentMethodUpdateDto
    {
        public int Id { get; set; }
        public string MethodName { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
