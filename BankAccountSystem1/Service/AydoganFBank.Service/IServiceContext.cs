
using AydoganFBank.AccountManagement.Service;

namespace AydoganFBank.Service
{
    public interface IServiceContext
    {
        IAccountManager AccountManager { get; }
        ICompanyManager CompanyManager { get; }
        ICreditCardManager CreditCardManager { get; }
        IPersonManager PersonManager { get; }
    }
}
