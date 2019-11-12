using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.TransactionManagement.Api
{
    public interface ITransactionOwner
    {
        int OwnerId { get; }
        TransactionOwnerType OwnerType { get; }
        string TransactionDetailDisplayName { get; }
        string AssetsUnit { get; }
    }

    public interface ITransactionOwnerWithDetails : ITransactionOwner
    {
    }
}
