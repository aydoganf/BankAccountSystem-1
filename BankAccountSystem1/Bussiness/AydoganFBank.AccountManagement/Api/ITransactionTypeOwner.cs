using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ITransactionTypeOwner
    {
        ITransactionTypeInfo TransactionType { get; set; }
    }
}
