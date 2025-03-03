using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Domain.Enums
{
    public enum PaymentMethod
    {
        [Display(Name = "Cash")]
        Cash = 1,

        [Display(Name = "Credit Card")]
        CreditCard = 2,

        [Display(Name = "QR Code Payment")]
        QRCodePayment = 3,

        [Display(Name = "eWallet")]
        EWallet = 4,

        [Display(Name = "Bank Transfer")]
        BankTransfer = 5,

        [Display(Name = "DuitNow")]
        DuitNow = 6
    }
}