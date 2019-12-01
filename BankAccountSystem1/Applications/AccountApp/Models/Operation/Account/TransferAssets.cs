using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountApp.Models.Operation.Account
{
    public class TransferAssets
    {
        public string ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public AccountOverview AccountOverview { get; set; }
    }
}