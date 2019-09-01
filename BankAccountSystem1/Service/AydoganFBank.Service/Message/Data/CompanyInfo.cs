using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Message.Data
{
    public class CompanyInfo
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public PersonInfo ResponsablePerson { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxNumber { get; set; }
        public AccountInfo Account { get; set; }
    }
}
