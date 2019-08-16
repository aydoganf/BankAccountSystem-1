using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ITransactionStatusInfo
    {
        string StatusName { get; set; }
        int StatusId { get; set; }
        string StatusKey { get; set; }
    }
}
