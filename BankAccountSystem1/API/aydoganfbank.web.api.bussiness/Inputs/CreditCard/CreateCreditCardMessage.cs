using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aydoganfbank.web.api.bussiness.Inputs.CreditCard
{
    public class CreateCreditCardMessage
    {
        public decimal Limit { get; set; }
        public int ExtreDate { get; set; }
        public int Type { get; set; }
        public string ValidMonth { get; set; }
        public string ValidYear { get; set; }
        public string SecurityCode { get; set; }
        public bool IsInternetUsageOpen { get; set; }
        public CreditCardOwnerInfo CreditCardOwner { get; set; }
    }

    public class CreditCardOwnerInfo
    {
        public int OwnerType { get; set; }
        public int OwnerId { get; set; }
    }
}
