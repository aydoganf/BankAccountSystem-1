using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.TransactionManagement.Api
{
    public interface ITransactionHolder
    {
        ITransactionInfo TransactionInfo { get; }
        DateTime CreateDate { get; }
    }
}
