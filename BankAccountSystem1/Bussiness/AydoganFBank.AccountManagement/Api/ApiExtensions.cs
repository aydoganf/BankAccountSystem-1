using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Api
{
    internal static class ApiExtensions
    {
        public static int ToInt(this AccountOwnerType accountOwnerType)
        {
            return Convert.ToInt32(accountOwnerType);
        }

        public static int ToInt(this TransactionOwnerType transactionOwnerType)
        {
            return Convert.ToInt32(transactionOwnerType);
        }
    }
}
