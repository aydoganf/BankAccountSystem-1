using AydoganFBank.Service.Dispatcher.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountApp.Models.Operation.CreditCard
{
    public class CreditCardDischarge
    {
        public CreditCardInfo CreditCard { get; set; }
        public CreditCardExtreInfo CreditCardExtre { get; set; }
        public List<AccountInfo> Accounts { get; set; }

        public List<SelectListItem> AccountsForUI
        {
            get
            {
                var first = new SelectListItem() { Text = "Select an account", Value = "0" };
                var list = Accounts.Select(i => new SelectListItem()
                {
                    Text = $"{i.AccountNumber} - {i.Balance} {i.AccountType.AssetsUnit}",
                    Value = i.Id.ToString()
                }).ToList();

                list.Insert(0, first);
                return list;
            }
        }

        public AccountInfo SelectedAccount { get; set; }
        public int SelectedAccountId { get; set; }
        public decimal DischargeAmount { get; set; }

        public CreditCardDischarge(CreditCardInfo creditCard, CreditCardExtreInfo creditCardExtre, List<AccountInfo> accounts)
        {
            CreditCard = creditCard;
            CreditCardExtre = creditCardExtre;
            Accounts = accounts;
        }

        public CreditCardDischarge()
        {
        }
    }
}