using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.TransactionManagement.Api
{
    public interface ITransactionTypeOwner
    {
        ITransactionTypeInfo TransactionType { get; set; }
    }
}
