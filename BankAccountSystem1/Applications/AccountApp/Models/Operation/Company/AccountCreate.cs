using AydoganFBank.Service.Dispatcher.Data;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AccountApp.Models.Operation.Company
{
    public class AccountCreate
    {
        public string CompanyName { get; set; }
        public int AccountTypeId { get; set; }

        public List<AccountTypeInfo> AccountTypeList { get; set; }
        public List<SelectListItem> AccountTypesForUI { get; set; }

        public void SetCompanyName(string companyName)
        {
            CompanyName = companyName;
        }

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