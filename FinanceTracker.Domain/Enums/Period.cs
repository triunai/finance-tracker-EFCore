using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Domain.Enums
{

    // this is in caps, in ur db its daily,weekly,monthly
    public enum Period
    {
        Daily,
        Monthly,
        Weekly,
        Yearly
    }
}
