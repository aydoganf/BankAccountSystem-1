using AydoganFBank.AccountManagement.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Message.Data
{
    public class AccountOwner
    {
        public AccountOwnerType OwnerType { get; set; }
        public int OwnerId { get; set; }
        public string DisplayName { get; set; }
    }
}
