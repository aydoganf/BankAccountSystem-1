using AydoganFBank.Service.Dispatcher.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountApp.Models
{
    public class PersonOverview
    {
        public List<AccountInfo> Accounts { get; private set; }
        public List<CreditCardInfo> CreditCards { get; private set; }
        public List<CompanyInfo> Companies { get; private set; }

        public List<AccountOverview> AccountOverviews { get; private set; }

        public PersonOverview(
            List<AccountInfo> accounts, 
            List<CreditCardInfo> creditCards,
            List<CompanyInfo> companies)
        {
            Accounts = accounts;
            CreditCards = creditCards;
            Companies = companies;

            ComputeAccountOverviews();
        }

        private void ComputeAccountOverviews()
        {
            AccountOverviews = new List<AccountOverview>();
            foreach (var account in Accounts)
            {
                var creditCard = CreditCards.FirstOrDefault(
                    cc => 
                        cc.CreditCardOwner.CreditCardOwnerType == AydoganFBank.AccountManagement.Api.CreditCardOwnerType.Account && 
                        cc.CreditCardOwner.OwnerId == account.Id);

                AccountOverviews.Add(new AccountOverview(account, creditCard));
            }
        }
    }
}