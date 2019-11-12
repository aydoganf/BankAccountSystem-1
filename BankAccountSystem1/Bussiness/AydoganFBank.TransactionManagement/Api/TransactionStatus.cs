using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.TransactionManagement.Api
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
