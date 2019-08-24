using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Context.IoC;
using System;
using System.Collections.Generic;

namespace AydoganFBank.AccountManagement.Managers
{
    public class AccountManager : IAccountManager
    {
        #region IoC
        private readonly ICoreContext coreContext;


        public AccountManager(
            ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }
        #endregion

        #region Account

        internal AccountDomainEntity CreateAccount(AccountTypeDomainEntity accountType, IAccountOwner accountOwner)
        {
            var account = coreContext.New<AccountDomainEntity>()
                .With(accountType, accountOwner);

            account.Insert();
            return account;
        }

        internal AccountDomainEntity CreatePersonAccount(string accountTypeKey, int personId)
        {
            var accountType = coreContext.Query<IAccountTypeRepository>().GetByKey(accountTypeKey);
            var person = coreContext.Query<IPersonRepository>().GetById(personId);
            return CreateAccount(accountType, person);
        }

        internal AccountDomainEntity CreateCompanyAccount(string accountTypeKey, int companyId)
        {
            var accountType = coreContext.Query<IAccountTypeRepository>().GetByKey(accountTypeKey);
            var company = coreContext.Query<ICompanyRepository>().GetById(companyId);
            return CreateAccount(accountType, company);
        }

        internal AccountDomainEntity GetAccountById(int accountId)
        {
            return coreContext.Query<IAccountRepository>().GetById(accountId);
        }

        internal AccountDomainEntity GetAccountByAccountNumber(string accountNumber)
        {
            return coreContext.Query<IAccountRepository>().GetByAccountNumber(accountNumber);
        }

        internal AccountDomainEntity WithdrawMoneyFromOwn(int accountId, decimal amount)
        {
            AccountTransactionDomainEntity transaction = null;
            AccountDomainEntity account = null;
            bool isWithdrawOK = false;

            try
            {
                account = GetAccountById(accountId);

                transaction = coreContext
                    .New<AccountTransactionDomainEntity>()
                    .With(account, null, amount, TransactionTypeEnum.AccountItself, TransactionStatusEnum.InProgress, account);

                transaction.Insert();

                isWithdrawOK = account.Withdraw(amount);

                transaction.SetStatus(TransactionStatusEnum.Succeeded);
                transaction.Save();
                var transactionDetail = transaction.CreateTransactionDetail(TransactionDirection.Out);
                transactionDetail.Insert();
            }
            catch (Exception ex)
            {
                if (transaction != null && isWithdrawOK == false)
                {
                    transaction.SetStatus(TransactionStatusEnum.Failed);
                    transaction.Save();
                }

                throw ex;
            }

            return account;
        }

        internal AccountDomainEntity DepositToOwnAccount(int accountId, decimal amount)
        {
            AccountTransactionDomainEntity transaction = null;
            AccountDomainEntity account = null;
            bool isDepositOk = false;
            try
            {
                account = GetAccountById(accountId);

                transaction = coreContext.New<AccountTransactionDomainEntity>()
                    .With(null, account, amount, TransactionTypeEnum.AccountItself, TransactionStatusEnum.InProgress, account);

                transaction.Insert();
                isDepositOk = account.Deposit(amount);

                transaction.SetStatus(TransactionStatusEnum.Succeeded);
                transaction.Save();

                var transactionDetail = transaction.CreateTransactionDetail(TransactionDirection.In);
                transactionDetail.Insert();
            }
            catch (Exception ex)
            {
                if (transaction != null && isDepositOk == false)
                {
                    transaction.SetStatus(TransactionStatusEnum.Failed);
                    transaction.Save();
                }

                throw ex;
            }
            
            return account;
        }

        internal void TransferAssets(int fromAccountId, int toAccountId, decimal amount, TransactionTypeEnum transactionType)
        {
            var fromAccount = GetAccountById(fromAccountId);
            var toAccount = GetAccountById(toAccountId);
            TransferAssets(fromAccount, toAccount, amount, transactionType);
        }

        /// <summary>
        /// Does assets transfer from one account to another account with given TransactionType
        /// </summary>
        /// <param name="fromAccount"></param>
        /// <param name="toAccount"></param>
        /// <param name="amount"></param>
        /// <param name="transactionTypeEnum"></param>
        private void TransferAssets(
            AccountDomainEntity fromAccount, 
            AccountDomainEntity toAccount, 
            decimal amount, 
            TransactionTypeEnum transactionTypeEnum)
        {
            AccountTransactionDomainEntity transaction = null;
            bool isWithdrawOk = false;
            bool isDepositOk = false;

            try
            {
                if (fromAccount.AccountType.AccountTypeKey != toAccount.AccountType.AccountTypeKey)
                    throw new AccountManagementException.DifferentAccountTypesCouldNotTransferAssetsToEachOther(string
                        .Format("FromAccountType: {0} - ToAccountType: {1}", fromAccount.AccountType.AccountTypeName, toAccount.AccountType.AccountTypeName));

                transaction = coreContext.New<AccountTransactionDomainEntity>()
                    .With(fromAccount, toAccount, amount, transactionTypeEnum, TransactionStatusEnum.InProgress, null);
                transaction.Insert();

                isWithdrawOk = fromAccount.Withdraw(amount);
                isDepositOk = toAccount.Deposit(amount);

                transaction.SetStatus(TransactionStatusEnum.Succeeded);
                transaction.Save();

                var transactionDetailIn = transaction.CreateTransactionDetail(TransactionDirection.In);
                transactionDetailIn.Insert();

                var transactionDetailOut = transaction.CreateTransactionDetail(TransactionDirection.Out);
                transactionDetailOut.Insert();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    if (isWithdrawOk && isDepositOk == false)
                    {
                        fromAccount.Deposit(amount);
                        transaction.SetStatus(TransactionStatusEnum.Failed);

                        transaction.Save();
                    }
                }

                throw ex;
            }
        }
        

        #endregion
        
        #region AccountType

        public AccountTypeDomainEntity GetAccountTypeById(int id)
        {
            return coreContext.Query<IAccountTypeRepository>().GetById(id);
        }

        public AccountTypeDomainEntity GetAccountTypeByKey(string key)
        {
            return coreContext.Query<IAccountTypeRepository>().GetByKey(key);
        }

        #endregion

        #region TransactionStatus

        public TransactionStatusDomainEntity GetTransactionStatusById(int id)
        {
            return coreContext.Query<ITransactionStatusRepository>().GetById(id);
        }

        public TransactionStatusDomainEntity GetTransactionStatusByKey(string key)
        {
            return coreContext.Query<ITransactionStatusRepository>().GetByKey(key);
        }

        #endregion

        #region TransactionType

        public TransactionTypeDomainEntity GetTransactionTypeById(int id)
        {
            return coreContext.Query<ITransactionTypeRepository>().GetById(id);
        }

        public TransactionTypeDomainEntity GetTransactionTypeByKey(string key)
        {
            return coreContext.Query<ITransactionTypeRepository>().GetByKey(key);
        }

        #endregion

        #region TransactionOrder

        private TransactionOrderDomainEntity CreateTransactionOrder(
            TransactionTypeEnum transactionType,
            string orderDescription,
            DateTime operationDate,
            AccountDomainEntity fromAccount,
            AccountDomainEntity toAccount,
            decimal amount)
        {
            TransactionOrderDomainEntity transactionOrder = coreContext
                .New<TransactionOrderDomainEntity>()
                .With(transactionType, orderDescription, operationDate, fromAccount, toAccount, amount, TransactionStatusEnum.Created);
            transactionOrder.Insert();

            return transactionOrder;
        }

        private List<TransactionOrderDomainEntity> GetTransactionOrderListByAccount(AccountDomainEntity account)
        {
            return coreContext.Query<ITransactionOrderRepository>().GetListByFromAccount(account);
        }


        public List<TransactionOrderDomainEntity> GetTransactionOrderListByAccount(int accountId)
        {
            AccountDomainEntity account = GetAccountById(accountId);
            return GetTransactionOrderListByAccount(account);
        }

        public List<TransactionOrderDomainEntity> GetAllTransactionOrders(DateTime operationDate)
        {
            return coreContext.Query<ITransactionOrderRepository>().GetListByOperationDate(operationDate);
        }

        public List<TransactionOrderDomainEntity> GetUncompletedTransactionOrders(DateTime operationDate)
        {
            return coreContext.Query<ITransactionOrderRepository>().GetUncompletedListByOperationDate(operationDate);
        }

        public TransactionOrderDomainEntity CreateTransactionOrder(
            TransactionTypeEnum transactionType,
            string orderDescription,
            DateTime operationDate,
            int fromAccountId,
            int toAccountId,
            decimal amount)
        {
            var fromAccount = GetAccountById(fromAccountId);
            var toAccount = GetAccountById(toAccountId);
            return CreateTransactionOrder(transactionType, orderDescription, operationDate, fromAccount, toAccount, amount);
        }

        internal void DoTransactionOrder(TransactionOrderDomainEntity transactionOrder)
        {
            try
            {
                TransferAssets(
                    transactionOrder.FromAccount,
                    transactionOrder.ToAccount,
                    transactionOrder.Amount,
                    TransactionTypeEnum.FromAccountToAccount);

                transactionOrder.SetStatus(TransactionStatusEnum.Succeeded);
            }
            catch (Exception)
            {
                transactionOrder.SetStatus(TransactionStatusEnum.Failed);
            }
        }
        #endregion
        

        #region API Implementations
        IAccountInfo IAccountManager.CreatePersonAccount(string accountTypeKey, int personId)
        {
            return CreatePersonAccount(accountTypeKey, personId);
        } 

        IAccountInfo IAccountManager.CreateCompanyAccount(string accountTypeKey, int companyId)
        {
            return CreateCompanyAccount(accountTypeKey, companyId);
        }

        IAccountInfo IAccountManager.GetAccountInfo(int accountId)
        {
            return GetAccountById(accountId);
        }

        IAccountInfo IAccountManager.GetAccountInfoByAccountNumber(string accountNumber)
        {
            return GetAccountByAccountNumber(accountNumber);
        }

        IAccountInfo IAccountManager.WithdrawMoneyFromOwn(int accountId, decimal amount)
        {
            return WithdrawMoneyFromOwn(accountId, amount);
        }

        IAccountInfo IAccountManager.DepositToOwnAccount(int accountId, decimal amount)
        {
            return DepositToOwnAccount(accountId, amount);
        }

        void IAccountManager.TransferAssets(int fromAccountId, int toAccountId, decimal amount, TransactionTypeEnum transactionType)
        {
            TransferAssets(fromAccountId, toAccountId, amount, transactionType);
        }

        IAccountTypeInfo IAccountManager.GetAccountTypeInfo(int accountTypeId)
        {
            return GetAccountTypeById(accountTypeId);
        }

        IAccountTypeInfo IAccountManager.GetAccountTypeByKey(string key)
        {
            return GetAccountTypeByKey(key);
        }
        #endregion
    }
}
