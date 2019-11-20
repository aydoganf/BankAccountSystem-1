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
        public List<CreditCardExtreInfo> CreditCardExtreList { get; set; }
        public CreditCardExtreInfo CurrentExtre { get; set; }

        public List<TransactionDetailInfo> CurrentTransactionDetailList { get; set; }
        public List<CreditCardPaymentInfo> CurrentCreditCardPaymentList { get; set; }


        public CreditCardExtreOvierview ExtreOvierview { get; set; }

        public decimal CurrentTermTotalTransactionAmount { get; set; }


        public CreditCardOverview(CreditCardInfo creditCard)
        {
            CreditCard = creditCard;
        }

        public void SetTransantionDetails(List<TransactionDetailInfo> transactionDetails)
        {
            TransactionDetailList = transactionDetails;
        }

        public void SetCurrentExtreCreditCardPaymentList(List<CreditCardPaymentInfo> creditCardPayments)
        {
            CurrentCreditCardPaymentList = creditCardPayments;
        }

        public void SetCurrentTransactionDetailList(List<TransactionDetailInfo> transactionDetails)
        {
            CurrentTransactionDetailList = transactionDetails;

            decimal inWay = CurrentTransactionDetailList
                .Where(
                td =>
                    td.TransactionDirection == AydoganFBank.AccountManagement.Api.TransactionDirection.In)
                .Sum(td => td.Amount);

            decimal outWay = CurrentTransactionDetailList
                .Where(
                td =>
                    td.TransactionDirection == AydoganFBank.AccountManagement.Api.TransactionDirection.Out)
                .Sum(td => td.Amount);

            CurrentTermTotalTransactionAmount = outWay - inWay;
        }

        public void SetCurrentExtre(CreditCardExtreInfo creditCardExtre)
        {
            CurrentExtre = creditCardExtre;
        }

        public void SetCreditCardExtreList(List<CreditCardExtreInfo> creditCardExtres)
        {
            CreditCardExtreList = creditCardExtres;
        }

        public void SetExtreDetails(
            List<CreditCardExtreInfo> creditCardExtres, 
            List<CreditCardPaymentInfo> creditCardPayments,
            List<TransactionDetailInfo> transactionDetails)
        {
            ExtreOvierview = new CreditCardExtreOvierview(CreditCard, creditCardExtres, creditCardPayments, transactionDetails);
        }
    }
}