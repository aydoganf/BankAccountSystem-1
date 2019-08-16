using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ITransactionHolder
    {
        ITransactionInfo TransactionInfo { get; }
        DateTime CreateDate { get; }
    }
}
