using AydoganFBank.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Domain
{
    public class AccountTypeDomainEntity : IDomainEntity
    {
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountTypeKey { get; set; }
    }
}
