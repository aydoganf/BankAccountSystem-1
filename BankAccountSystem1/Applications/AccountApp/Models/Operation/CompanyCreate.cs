using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountApp.Models.Operation
{
    public class CompanyCreate
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxNumber { get; set; }
    }
}