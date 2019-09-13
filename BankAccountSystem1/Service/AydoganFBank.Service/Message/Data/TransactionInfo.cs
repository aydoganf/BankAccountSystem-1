using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Message.Data
{
    public class TransactionInfo
    {
        public TransactionOwner TransactionOwner { get; set; }
        public TransactionOwner FromTransactionOwner { get; set; }
        public TransactionOwner ToTransactionOwner { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionTypeInfo TransactionType { get; set; }
        public TransactionStatusInfo TransactionStatus { get; set; }
    }
}
