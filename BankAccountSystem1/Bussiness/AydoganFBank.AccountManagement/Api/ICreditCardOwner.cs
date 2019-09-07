using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ICreditCardOwner
    {
        int OwnerId { get; }
        CreditCardOwnerType CreditCardOwnerType { get; }
        string AssetsUnit { get; }
    }

    public enum CreditCardOwnerType
    {
        Account = 1
    }
}
