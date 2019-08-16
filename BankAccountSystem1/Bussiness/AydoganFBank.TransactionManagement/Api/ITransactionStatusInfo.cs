using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.TransactionManagement.Api
{
    public interface ITransactionStatusInfo
    {
        string StatusName { get; set; }
        int StatusId { get; set; }
        string StatusKey { get; set; }
    }
}
