using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Context.IoC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AydoganFBank.AccountManagement.Managers
{
    public class AccountManager : IAccountManager
    {
        #region IoC
        private readonly ICoreContext coreContext;
        private readonly PersonManager personManager;
        private readonly CompanyManager companyManager;

        public AccountManager(
            ICoreContext coreContext,
            PersonManager personManager,
            CompanyManager companyManager)
        {
            this.coreContext = coreContext;
            this.personManager = personManager;
            this.companyManager = companyManager;

            this.coreContext.Logger.Info("AccountManager created.", this.coreContext.GetContainerInfo());
        }
        #endregion

        #region Account

        internal List<AccountDomainEntity> GetAccountsByPerson(PersonDomainEntity person)
        {
            List<AccountDomainEntity> accounts = coreContext.Query<IAccountRepository>().GetListByOwner(person);

            var companies = companyManager.GetCompanyByResponsableId(person.PersonId);
            if (companies != null)
            {
                companies.ForEach(company => 
                {
                    accounts.AddRange(coreContext.Query<IAccountRepository>().GetListByOwner(company));
                });
            }

            return accounts;
        }

        internal List<AccountDomainEntity> GetAccountsByPerson(int personId)
        {
            var person = personManager.GetPersonById(personId);
            return GetAccountsByPerson(person);
        }

        internal object DeleteAccount(int accountId)
        {
            var account = GetAccountById(accountId);
            account.Delete();
            return new object();
        }

        internal AccountDomainEntity CreateAccount(AccountTypeDomainEntity accountType, IAccountOwner accountOwner, string accountNumber)
        {
            var account = coreContext.New<AccountDomainEntity>()
                .With(accountType, accountOwner, accountNumber);

            account.Insert();
            return account;
        }

        internal AccountDomainEntity CreatePersonAccount(string accountTypeKey, int personId)
        {
            var accountType = coreContext.Query<IAccountTypeRepository>().GetByKey(accountTypeKey);
            var person = coreContext.Query<IPersonRepository>().GetById(personId);
            if (coreContext.Query<IAccountRepository>().HasOwnerAccountByType(person, accountType))
            {
                throw new AccountManagementException.AccountOwnerHasAlreadyAnAccountWithGivenAccountType(string.Format("{0} - {1}",
                    nameof(accountType), accountTypeKey));
            }

            var accountNumber = coreContext.Query<IAccountRepository>().GetNextAccountNumber();
            return CreateAccount(accountType, person, accountNumber);
        }

        internal AccountDomainEntity CreateCompanyAccount(string accountTypeKey, int companyId)
        {
            var accountNumber = coreContext.Query<IAccountRepository>().GetNextAccountNumber();
            var accountType = coreContext.Query<IAccountTypeRepository>().GetByKey(accountTypeKey);
            var company = coreContext.Query<ICompanyRepository>().GetById(companyId);
            
            return CreateAccount(accountType, company, accountNumber);
        }

        internal AccountDomainEntity GetAccountById(int accountId)
        {
            return coreContext.Query<IAccountRepository>().GetById(accountId);
        }

        internal AccountDomainEntity GetAccountByAccountNumber(string accountNumber)
        {
            var account = coreContext.Query<IAccountRepository>().GetByAccountNumber(accountNumber);

            if (account == null)
                throw new AccountManagementException.AccountCouldNotFoundWithGivenAccountNumber(accountNumber);

            return account;
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
                    .With(fromAccount, toAccount, amount, transactionTypeEnum, TransactionStatusEnum.InProgress, fromAccount);
                transaction.Insert();

                isWithdrawOk = fromAccount.Withdraw(amount, false);
                isDepositOk = toAccount.Deposit(amount, false);

                transaction.SetStatus(TransactionStatusEnum.Succeeded);

                var transactionDetailIn = transaction.CreateTransactionDetail(TransactionDirection.In);
                transactionDetailIn.Insert(false);

                var transactionDetailOut = transaction.CreateTransactionDetail(TransactionDirection.Out);
                transactionDetailOut.Insert(false);

                coreContext.Commit();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    if (isWithdrawOk && isDepositOk == false)
                    {
                        fromAccount.Deposit(amount);
                        transaction.SetStatus(TransactionStatusEnum.Failed);
                        coreContext.Commit();
                    }
                }

                throw ex;
            }
        }


        #endregion

        #region AccountType

        internal AccountTypeDomainEntity GetAccountTypeById(int id)
        {
            return coreContext.Query<IAccountTypeRepository>().GetById(id);
        }

        internal AccountTypeDomainEntity GetAccountTypeByKey(string key)
        {
            return coreContext.Query<IAccountTypeRepository>().GetByKey(key);
        }

        internal List<AccountTypeDomainEntity> GetAccountTypeList()
        {
            return coreContext.Query<IAccountTypeRepository>().GetAccountTypeList();
        }
        #endregion

        #region TransactionStatus

        internal TransactionStatusDomainEntity GetTransactionStatusById(int id)
        {
            return coreContext.Query<ITransactionStatusRepository>().GetById(id);
        }

        internal TransactionStatusDomainEntity GetTransactionStatusByKey(string key)
        {
            return coreContext.Query<ITransactionStatusRepository>().GetByKey(key);
        }

        #endregion

        #region TransactionType

        internal TransactionTypeDomainEntity GetTransactionTypeById(int id)
        {
            return coreContext.Query<ITransactionTypeRepository>().GetById(id);
        }

        internal TransactionTypeDomainEntity GetTransactionTypeByKey(string key)
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


        internal List<TransactionOrderDomainEntity> GetTransactionOrderListByAccount(int accountId)
        {
            AccountDomainEntity account = GetAccountById(accountId);
            return GetTransactionOrderListByAccount(account);
        }

        internal List<TransactionOrderDomainEntity> GetAllTransactionOrders(DateTime operationDate)
        {
            return coreContext.Query<ITransactionOrderRepository>().GetListByOperationDate(operationDate);
        }

        internal List<TransactionOrderDomainEntity> GetUncompletedTransactionOrders(DateTime operationDate)
        {
            return coreContext.Query<ITransactionOrderRepository>().GetUncompletedListByOperationDate(operationDate);
        }

        internal TransactionOrderDomainEntity CreateTransactionOrder(
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
        List<IAccountInfo> IAccountManager.GetAccountsByPerson(int personId) => GetAccountsByPerson(personId).Cast<IAccountInfo>().ToList();

        object IAccountManager.DeleteAccount(int accountId) => DeleteAccount(accountId);

        IAccountInfo IAccountManager.CreatePersonAccount(string accountTypeKey, int personId) => CreatePersonAccount(accountTypeKey, personId);

        IAccountInfo IAccountManager.CreateCompanyAccount(string accountTypeKey, int companyId) => CreateCompanyAccount(accountTypeKey, companyId);

        IAccountInfo IAccountManager.GetAccountInfo(int accountId)
        {
            var account = GetAccountById(accountId) as IAccountInfo;
            return account;
        }

        IAccountInfo IAccountManager.GetAccountInfoByAccountNumber(string accountNumber) => GetAccountByAccountNumber(accountNumber);

        IAccountInfo IAccountManager.WithdrawMoneyFromOwn(int accountId, decimal amount) => WithdrawMoneyFromOwn(accountId, amount);

        IAccountInfo IAccountManager.DepositToOwnAccount(int accountId, decimal amount) => DepositToOwnAccount(accountId, amount);

        object IAccountManager.TransferAssets(int fromAccountId, int toAccountId, decimal amount, TransactionTypeEnum transactionType)
        {
            TransferAssets(fromAccountId, toAccountId, amount, transactionType);
            return new object();
        }

        IAccountTypeInfo IAccountManager.GetAccountTypeInfo(int accountTypeId) => GetAccountTypeById(accountTypeId);

        IAccountTypeInfo IAccountManager.GetAccountTypeByKey(string key) => GetAccountTypeByKey(key);

        List<IAccountTypeInfo> IAccountManager.GetAccountTypeList() => GetAccountTypeList().Cast<IAccountTypeInfo>().ToList();
        #endregion
    }
}
