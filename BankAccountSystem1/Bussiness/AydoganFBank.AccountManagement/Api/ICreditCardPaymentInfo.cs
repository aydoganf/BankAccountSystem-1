using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ICreditCardPaymentInfo
    {
        int CreditCardPaymentId { get; }
        int InstalmentIndex { get; }
        decimal Amount { get; }
        string Description { get; }
        DateTime CreateDate { get; }
        DateTime InstalmentDate { get; }
        ITransactionInfo AccountTransaction { get; }
    }
}
