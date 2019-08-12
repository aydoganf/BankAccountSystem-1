using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ITransactionStatusOwner
    {
        ITransactionStatusInfo TransactionOrderStatus { get; set; }
    }
}
