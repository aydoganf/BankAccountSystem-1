using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ITransactionOwner
    {
        int OwnerId { get; }
        TransactionOwnerType OwnerType { get; }
        string TransactionDetailDisplayName { get; }
        string AssetsUnit { get; }
    }
}
