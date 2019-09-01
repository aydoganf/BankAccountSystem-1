using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Service.Builder;
using AydoganFBank.Service.Message.Data;
using AydoganFBank.Service.Services.APIs;

namespace AydoganFBank.Service.Services
{
    public class AccountManagerService : IAccountManagerService
    {
        internal readonly IAccountManager accountManager;
        internal readonly ServiceDataBuilder dataBuilder;

        public AccountManagerService(IAccountManager accountManager, ServiceDataBuilder dataBuilder)
        {
            this.accountManager = accountManager;
            this.dataBuilder = dataBuilder;
        }

        public AccountInfo CreateCompanyAccount(string accountTypeKey, int companyId)
        {
            var account = accountManager.CreateCompanyAccount(accountTypeKey, companyId);
            return dataBuilder.BuildAccountInfo(account);
        }

        public AccountInfo CreatePersonAccount(string accountTypeKey, int personId)
        {
            var account = accountManager.CreatePersonAccount(accountTypeKey, personId);
            return dataBuilder.BuildAccountInfo(account);
        }

        public AccountInfo DepositToOwnAccount(int accountId, decimal amount)
        {
            var account = accountManager.DepositToOwnAccount(accountId, amount);
            return dataBuilder.BuildAccountInfo(account);
        }

        public AccountInfo GetAccountInfo(int accountId)
        {
            var account = accountManager.GetAccountInfo(accountId);
            return dataBuilder.BuildAccountInfo(account);
        }

        public AccountInfo GetAccountInfoByAccountNumber(string accountNumber)
        {
            var account = accountManager.GetAccountInfoByAccountNumber(accountNumber);
            return dataBuilder.BuildAccountInfo(account);
        }

        public AccountTypeInfo GetAccountTypeByKey(string key)
        {
            var accountType = accountManager.GetAccountTypeByKey(key);
            return dataBuilder.BuildAccountTypeInfo(accountType);
        }

        public AccountTypeInfo GetAccountTypeInfo(int accountTypeId)
        {
            var accountType = accountManager.GetAccountTypeInfo(accountTypeId);
            return dataBuilder.BuildAccountTypeInfo(accountType);
        }

        public object TransferAssets(int fromAccountId, int toAccountId, decimal amount, TransactionTypeEnum transactionType)
        {
            return accountManager.TransferAssets(fromAccountId, toAccountId, amount, transactionType);
        }

        public AccountInfo WithdrawMoneyFromOwn(int accountId, decimal amount)
        {
            var account = accountManager.WithdrawMoneyFromOwn(accountId, amount);
            return dataBuilder.BuildAccountInfo(account);
        }
    }
}
