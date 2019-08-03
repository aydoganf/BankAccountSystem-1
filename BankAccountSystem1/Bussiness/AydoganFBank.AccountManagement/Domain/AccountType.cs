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

        public int Id => AccountTypeId;

        public void Insert(bool forceToınsertDb = true)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
