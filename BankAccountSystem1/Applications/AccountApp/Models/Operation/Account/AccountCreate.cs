using AydoganFBank.Service.Dispatcher.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountApp.Models.Operation.Account
{
    public class AccountCreate
    {
        public int AccountTypeId { get; set; }

        public List<AccountTypeInfo> AccountTypeList { get; set; }
        public List<SelectListItem> AccountTypesForUI { get; set; }

        public void SetAccountTypeList(List<AccountTypeInfo> accountTypes)
        {
            AccountTypeList = accountTypes;
            AccountTypesForUI = new List<SelectListItem>();

            foreach (var accountType in accountTypes)
            {
                AccountTypesForUI.Add(new SelectListItem()
                {
                    Text = accountType.TypeName,
                    Value = accountType.Id.ToString()
                });
            }

            AccountTypesForUI.Insert(0, new SelectListItem() { Text = "Select an account type", Value = "" });
        }
    }
}