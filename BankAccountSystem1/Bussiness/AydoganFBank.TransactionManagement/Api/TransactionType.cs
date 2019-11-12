using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.TransactionManagement.Api
{
    public enum TransactionTypeEnum
    {
        AccountItself  = 1,
        FromAccountToAccount,
        RealEstate,
        CreditCardPayment,
        CreditPayment,
    }
}
