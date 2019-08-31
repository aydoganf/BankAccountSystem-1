using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aydoganfbank.web.api.bussiness.Inputs.CreditCard
{
    public class DoPaymentWithSecurityInfosMessage
    {
        public string CreditCardNumber { get; set; }
        public string ValidMonth { get; set; }
        public string ValidYear { get; set; }
        public string SecurityCode { get; set; }
        public decimal Amount { get; set; }
        public int InstalmentCount { get; set; }
        public int ToAccountId { get; set; }
    }
}
