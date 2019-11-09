using AydoganFBank.Service.Dispatcher.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountApp.Models
{
    public class CreditCardOverview
    {
        public CreditCardInfo CreditCard { get; set; }
        public List<TransactionDetailInfo> TransactionDetailList { get; set; }
        public List<CreditCardPaymentInfo> CreditCardPaymentList { get; set; }

        public CreditCardOverview(CreditCardInfo creditCard)
        {
            CreditCard = creditCard;
        }

        public void SetTransantionDetails(List<TransactionDetailInfo> transactionDetails)
        {
            TransactionDetailList = transactionDetails;
        }

        public void SetCreditCardPaymentList(List<CreditCardPaymentInfo> creditCardPayments)
        {
            CreditCardPaymentList = creditCardPayments;
        }
    }
}