using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountApp.Models.Operation
{
    public class CompanyPhoneNumberChange
    {
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }

        public CompanyPhoneNumberChange()
        {
        }

        public CompanyPhoneNumberChange(string companyName, string phoneNumber)
        {
            CompanyName = companyName;
            PhoneNumber = phoneNumber;
        }
    }
}