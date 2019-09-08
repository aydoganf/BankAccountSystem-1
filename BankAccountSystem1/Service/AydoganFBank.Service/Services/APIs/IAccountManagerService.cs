using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service.Message.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Services.APIs
{
    public interface IAccountManagerService
    {
        AccountInfo GetAccountInfo(int accountId);
        AccountInfo GetAccountInfoByAccountNumber(string accountNumber);
        AccountTypeInfo GetAccountTypeInfo(int accountTypeId);
        AccountTypeInfo GetAccountTypeByKey(string key);

        AccountInfo CreatePersonAccount(string accountTypeKey, int personId);
        AccountInfo CreateCompanyAccount(string accountTypeKey, int companyId);

        AccountInfo WithdrawMoneyFromOwn(int accountId, decimal amount);
        AccountInfo DepositToOwnAccount(int accountId, decimal amount);

        object TransferAssets(int fromAccountId, int toAccountId, decimal amount, TransactionTypeEnum transactionType);
        object DeleteAccount(int accountId);
    }
}
