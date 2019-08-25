using AydoganFBank.AccountManagement.Api;

namespace AydoganFBank.AccountManagement.Service
{
    public interface ICreditCardManager
    {
        ICreditCardInfo DoCreditCardPayment(int creditCardId, decimal amount, int instalmentCount, ITransactionOwner toTransactionOwner);
        ICreditCardInfo CreateCreditCard(decimal limit, int extreDate, int type, string validMonth, string validYear, string securityCode, bool isInternetUsageOpen, ICreditCardOwner creditCardOwner);
    }
}
