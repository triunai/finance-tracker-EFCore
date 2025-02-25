using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Domain.Enums
{
    public enum PaymentMethod
    {
        Cash = 1,
        CreditCard = 2,
        QRCodePayment = 3,
        EWallet = 4,
        BankTransfer = 5,
        DuitNow = 6
    }
}