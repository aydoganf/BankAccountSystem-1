using AydoganFBank.Service.Dispatcher.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountApp.Models.Operation
{
    public class CreditCardCreate
    {
        public int AccountId { get; set; }
        public decimal Limit { get; set; }
        public int ExtreDay { get; set; }
        public int ValidMonth { get; set; }
        public int ValidYear { get; set; }
        public int SecurityCode { get; set; }
        public bool IsInternetUsageOpen { get; set; }

        public List<AccountInfo> AccountList { get; set; }
        public List<SelectListItem> AccountsForUI { get; set; }
        public List<SelectListItem> ExtreDayForUI { get; set; }
        public List<SelectListItem> ValidMonthForUI { get; set; }
        public List<SelectListItem> ValidYearForUI { get; set; }

        public CreditCardCreate()
        {
            ExtreDayForUI = new List<SelectListItem>();
            for (int i = 1; i <= 30; i++)
            {
                string day = i.ToString();
                if (i < 10)
                    day = $"0{i}";

                ExtreDayForUI.Add(new SelectListItem()
                {
                    Text = day,
                    Value = i.ToString()
                });
            }
            ExtreDayForUI.Insert(0, new SelectListItem() { Text = "Select an extre day" });

            ValidMonthForUI = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                string month = i.ToString();
                if (i < 10)
                    month = $"0{i}";

                ValidMonthForUI.Add(new SelectListItem()
                {
                    Text = month,
                    Value = i.ToString()
                });
            }
            ValidMonthForUI.Insert(0, new SelectListItem() { Text = "Select a valid month" });

            ValidYearForUI = new List<SelectListItem>();
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear; i <= currentYear + 10; i++)
            {
                ValidYearForUI.Add(new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            ValidYearForUI.Insert(0, new SelectListItem() { Text = "Select a valid year" });
        }

        public void SetAccountList(List<AccountInfo> accounts)
        {
            AccountList = accounts;

            AccountsForUI = new List<SelectListItem>();

            foreach (var account in accounts)
            {
                AccountsForUI.Add(new SelectListItem()
                {
                    Text = $"{account.AccountNumber} - {account.Balance} {account.AccountType.AssetsUnit}",
                    Value = account.Id.ToString()
                });
            }

            AccountsForUI.Insert(0, new SelectListItem() { Text = "Select an account", Value = "" });
        }

    }
}