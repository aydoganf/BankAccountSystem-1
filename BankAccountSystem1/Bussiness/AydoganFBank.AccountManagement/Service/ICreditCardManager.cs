using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.Context.DataAccess;
using System;
using System.Collections.Generic;

namespace AydoganFBank.AccountManagement.Service
{
    public interface ICreditCardManager : IDomianEntityManager
    {
        ICreditCardInfo DoCreditCardPayment(
            int creditCardId, 
            decimal amount, 
            int instalmentCount, 
            ITransactionOwner toTransactionOwner);

        ICreditCardInfo DoCreditCardPayment(
            int creditCardId, 
            decimal amount, 
            int instalmentCount, 
            int toAccountId);

        ICreditCardInfo CreateCreditCard(
            decimal limit, 
            int extreDate, 
            int type, 
            string validMonth, 
            string validYear, 
            string securityCode, 
            bool isInternetUsageOpen, 
            ICreditCardOwner creditCardOwner);

        ICreditCardInfo CreateAccountCreditCard(
            decimal limit, 
            int extreDay, 
            int type, 
            string validMonth, 
            string validYear, 
            string securityCode, 
            bool isInternetUsageOpen, 
            int accountId);

        ICreditCardInfo DoCreditCardPayment(
            string creditCardNumber, 
            string validMonth, 
            string validYear, 
            string securityCode,
            decimal amount, 
            int instalmentCount, 
            int toAccountId);

        List<ICreditCardInfo> GetCreditCardListByPerson(int personId);

        ICreditCardInfo GetCreditCardByAccount(string accountNumber);

        ICreditCardInfo GetCreditCardById(int creditCardId);

        List<ICreditCardPaymentInfo> GetCreditCardLastPaymentList(int creditCardId, DateTime fromDate);

        List<ICreditCardPaymentInfo> GetCreditCardLastExtrePayments(int creditCardId);
    }
}
