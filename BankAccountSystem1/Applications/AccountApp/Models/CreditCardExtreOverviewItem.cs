using AydoganFBank.Service.Dispatcher.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountApp.Models
{
    public class CreditCardExtreOverviewItem
    {
        public CreditCardExtreInfo CreditCardExtre { get; set; }
        public List<CreditCardPaymentInfo> CreditCardPaymentList { get; set; }

        public CreditCardExtreOverviewItem(CreditCardExtreInfo creditCardExtre)
        {
            CreditCardExtre = creditCardExtre;
        }

        public void SetPaymentList(List<CreditCardPaymentInfo> creditCardPayments)
        {
            CreditCardPaymentList = creditCardPayments;
        }
    }
}