using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.TransactionManagement.Api
{
    public interface ITransactionInfo
    {
        int Id { get; }
        ITransactionOwner TransactionOwner { get; }
        ITransactionOwner FromTransactionOwner { get; }
        ITransactionOwner ToTransactionOwner { get; }
        decimal Amount { get; }
        DateTime TransactionDate { get; }
        ITransactionTypeInfo TransactionType { get; }
        ITransactionStatusInfo TransactionStatus { get; }

        void SetStatus(TransactionStatusEnum transactionStatus);
    }
}
