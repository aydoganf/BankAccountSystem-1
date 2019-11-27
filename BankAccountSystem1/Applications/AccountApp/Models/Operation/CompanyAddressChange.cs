using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountApp.Models.Operation
{
    public class CompanyAddressChange
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }

        public CompanyAddressChange()
        {
        }

        public CompanyAddressChange(string companyName, string address)
        {
            CompanyName = companyName;
            Address = address;
        }
    }
}