using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ITransactionDetailInfo
    {
        int Id { get; }
        string Description { get; }
        ITransactionInfo TransactionInfo { get; }
        TransactionDirection TransactionDirection { get; } 
    }
}
