using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;

namespace AydoganFBank.AccountManagement.Service
{
    public interface ICreditCardManager
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
            int extreDate, 
            int type, 
            string validMonth, 
            string validYear, 
            string securityCode, 
            bool isInternetUsageOpen, 
            int accountId);

        ICreditCardInfo CreateCompanyCreditCard(
            decimal limit, 
            int extreDate, 
            int type, 
            string validMonth, 
            string validYear, 
            string securityCode, 
            bool isInternetUsageOpen, 
            int companyId);

        ICreditCardInfo DoCreditCardPayment(
            string creditCardNumber, 
            string validMonth, 
            string validYear, 
            string securityCode,
            decimal amount, 
            int instalmentCount, 
            int toAccountId);
    }
}
