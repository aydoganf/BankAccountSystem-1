using AydoganFBank.TransactionManagement.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.TransactionManagement.Service
{
    public interface ITransactionManager
    {
        ITransactionInfo MakeTransaction(
            ITransactionOwner from,
            ITransactionOwner to,
            decimal amount,
            TransactionTypeEnum transactionType,
            ITransactionOwner owner);

        ITransactionInfo SetTransactionStatus(
            ITransactionInfo transaction,
            TransactionStatusEnum status);

        ITransactionDetailInfo CreateTransactionDetail(ITransactionInfo transaction, TransactionDirection transactionDirection);
        
    }
}
