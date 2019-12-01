using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountApp.Models.Operation.CreditCard
{
    public class DoPayment
    {
        public string ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public int InstalmentCount { get; set; }

        public CreditCardOverview CreditCardOverview { get; set; }
    }
}