using AydoganFBank.AccountManagement.Service;

namespace AydoganFBank.Service
{
    public class ServiceContext : IServiceContext
    {
        public IAccountManager AccountManager { get; private set; }
        public ICompanyManager CompanyManager { get; private set; }
        public ICreditCardManager CreditCardManager { get; private set; }
        public IPersonManager PersonManager { get; private set; }

        public ServiceContext(IAccountManager accountManager, ICompanyManager companyManager, ICreditCardManager creditCardManager, IPersonManager personManager)
        {
            AccountManager = accountManager;
            CompanyManager = companyManager;
            CreditCardManager = creditCardManager;
            PersonManager = personManager;
        }
    }
}
