using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ICreditCardExtreInfo
    {
        int Id { get; }
        int Month { get; }
        string MonthName { get; }
        int Year { get; }
        decimal TotalPayment { get; }
        decimal MinPayment { get; }
        bool IsDischarged { get; }
        bool IsMinDischarged { get; }
        DateTime LastPaymentDate { get; }

        DateTime ExtreStartDate { get; }
        DateTime ExtreEndDate { get; }
    }
}
