using AydoganFBank.AccountManagement.Service;

namespace AydoganFBank.Service
{
    public interface IServiceContext
    {
        IAccountManager AccountManager { get; }
        IPersonManager PersonManager { get; }
        ICompanyManager CompanyManager { get; }
        ICreditCardManager CreditCardManager { get; }
        ITransactionManager TransactionManager { get; }
    }
}
