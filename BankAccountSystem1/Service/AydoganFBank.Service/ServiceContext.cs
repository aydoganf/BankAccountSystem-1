using AydoganFBank.AccountManagement.Service;

namespace AydoganFBank.Service
{
    public class ServiceContext : IServiceContext
    {
        public IAccountManager AccountManager { get; private set; }
        public IPersonManager PersonManager { get; private set; }
        public ICompanyManager CompanyManager { get; private set; }
        public ICreditCardManager CreditCardManager { get; private set; }
        public ITransactionManager TransactionManager { get; private set; }

        public ServiceContext(
            IAccountManager accountManager, IPersonManager personManager, ICompanyManager companyManager,
            ICreditCardManager creditCardManager, ITransactionManager transactionManager)
        {
            AccountManager = accountManager;
            PersonManager = personManager;
            CompanyManager = companyManager;
            CreditCardManager = creditCardManager;
            TransactionManager = transactionManager;
        }
    }
}
