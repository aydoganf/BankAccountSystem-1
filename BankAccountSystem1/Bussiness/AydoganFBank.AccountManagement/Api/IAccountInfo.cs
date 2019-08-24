using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Api
{
    public interface IAccountInfo
    {
        int Id { get; }
        string AccountNumber { get; }
        IAccountTypeInfo AccountType { get; }
        decimal Balance { get; }
        IAccountOwner AccountOwner { get; }

    }
}
