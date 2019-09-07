

using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.Context.DataAccess;

namespace AydoganFBank.AccountManagement.Service
{
    public interface IAccountManager : IDomianEntityManager
    {
        IAccountInfo CreatePersonAccount(string accountTypeKey, int personId);
        IAccountInfo CreateCompanyAccount(string accountTypeKey, int companyId);
        IAccountInfo GetAccountInfo(int accountId);
        IAccountInfo GetAccountInfoByAccountNumber(string accountNumber);
        IAccountInfo WithdrawMoneyFromOwn(int accountId, decimal amount);
        IAccountInfo DepositToOwnAccount(int accountId, decimal amount);

        object TransferAssets(int fromAccountId, int toAccountId, decimal amount, TransactionTypeEnum transactionType);

        IAccountTypeInfo GetAccountTypeInfo(int accountTypeId);
        IAccountTypeInfo GetAccountTypeByKey(string key);
    }
}
