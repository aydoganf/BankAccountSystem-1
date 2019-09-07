using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aydoganfbank.web.api.bussiness.Inputs.Company
{
    public class CreateCompanyMessage
    {
        public string CompanyName { get; set; }
        public int ResponsablePersonId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxNumber { get; set; }
    }
}
