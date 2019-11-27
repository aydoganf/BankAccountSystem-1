using AydoganFBank.Service.Dispatcher.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountApp.Models
{
    public class CompanyDetail
    {
        public CompanyInfo Company { get; set; }

        public List<AccountInfo> AccountList { get; set; }

        public List<CreditCardInfo> CreditCardList { get; set; }


        public CompanyDetail(CompanyInfo company, List<AccountInfo> accounts, List<CreditCardInfo> creditCards)
        {
            Company = company;
            AccountList = accounts;
            CreditCardList = creditCards;
        }
    }
}