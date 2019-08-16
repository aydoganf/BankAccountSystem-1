using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ITransactionInfo
    {
        ITransactionOwner TransactionOwner { get; }
        ITransactionOwner FromTransactionOwner { get; }
        ITransactionOwner ToTransactionOwner { get; }
        decimal Amount { get; }
        DateTime TransactionDate { get; }
        ITransactionTypeInfo TransactionType { get; }
        ITransactionStatusInfo TransactionStatus { get; }
    }
}
