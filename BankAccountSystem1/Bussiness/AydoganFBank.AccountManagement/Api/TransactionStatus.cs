using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Api
{
    public enum TransactionStatusEnum
    {
        Created = 1,
        Pending,
        InProgress,
        Failed,
        Succeeded,
        Canceled
    }
}
