using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Message.Data
{
    public class CreditCardInfo
    {
        public int Id { get; set; }
        public string CreditCardNumber { get; set; }
        public decimal Limit { get; set; }
        public int ExtreDay { get; set; }
        public decimal Debt { get; set; }
        public int Type { get; set; }
        public string ValidMonth { get; set; }
        public string ValidYear { get; set; }
        public string SecurityCode { get; set; }
        public bool IsInternetUsageOpen { get; set; }
        public CreditCardOwner CreditCardOwner { get; set; }

        public string CreditCardMaskedNumber { get; set; }
        public decimal UsableLimit { get; set; }
        public DateTime UntilValidDate { get; set; }
    }
}
