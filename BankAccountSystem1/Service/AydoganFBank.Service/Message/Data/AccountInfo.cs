using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Message.Data
{
    public class AccountInfo
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public AccountTypeInfo AccountType { get; set; }
        public AccountOwner AccountOwner { get; set; }
    }
}
