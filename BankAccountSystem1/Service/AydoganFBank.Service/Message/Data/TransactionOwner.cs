using AydoganFBank.AccountManagement.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Message.Data
{
    public class TransactionOwner
    {
        public int OwnerId { get; set; }
        public TransactionOwnerType OwnerType { get; set; }
        public string TransactionDetailDisplayName { get; set; }
        public string AssetsUnit { get; set; }
    }
}
