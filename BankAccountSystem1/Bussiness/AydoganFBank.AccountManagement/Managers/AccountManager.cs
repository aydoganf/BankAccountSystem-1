using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Common.IoC;
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

        #region privates
        private AccountDomainEntity CreateAccount(AccountTypeDomainEntity accountType, IAccountOwner accountOwner)
        {
            var account = coreContext.New<AccountDomainEntity>()
                .With(accountType, accountOwner);

            account.Insert();
            return account;
        }

        private AccountDomainEntity DepositToAccount(AccountDomainEntity account, decimal amount)
        {
            account.Deposit(amount);
            return account;
        }

        private AccountDomainEntity WithdrawFromAccount(AccountDomainEntity account, decimal amount)
        {
            account.Withdraw(amount);
            return account;
        }
        #endregion

        #region publics
        public AccountDomainEntity CreatePersonAccount(string accountTypeKey, int personId)
        {
            var accountType = coreContext.Query<IAccountTypeRepository>().GetByKey(accountTypeKey);
            var person = coreContext.Query<IPersonRepository>().GetById(personId);
            return CreateAccount(accountType, person);
        }

        public AccountDomainEntity CreateCompanyAccount(string accountTypeKey, int companyId)
        {
            var accountType = coreContext.Query<IAccountTypeRepository>().GetByKey(accountTypeKey);
            var company = coreContext.Query<ICompanyRepository>().GetById(companyId);
            return CreateAccount(accountType, company);
        }

        public AccountDomainEntity GetAccountById(int accountId)
        {
            return coreContext.Query<IAccountRepository>().GetById(accountId);
        }

        public AccountDomainEntity GetAccountByAccountNumber(string accountNumber)
        {
            return coreContext.Query<IAccountRepository>().GetByAccountNumber(accountNumber);
        }
        
        public AccountDomainEntity DepositToAccount(int accountId, decimal amount)
        {
            var account = coreContext.Query<IAccountRepository>().GetById(accountId);
            return DepositToAccount(account, amount);
        }

        public AccountDomainEntity DepositToAccount(string accountNumber, decimal amount)
        {
            var account = GetAccountByAccountNumber(accountNumber);
            return DepositToAccount(account, amount);
        }

        public AccountDomainEntity WithdrawFromAccount(int accountId, decimal amount)
        {
            var account = GetAccountById(accountId);
            return WithdrawFromAccount(account, amount);
        }

        public AccountDomainEntity WithdrawFromAccount(string accountNumber, decimal amount)
        {
            var account = GetAccountByAccountNumber(accountNumber);
            return WithdrawFromAccount(account, amount);
        }

        public AccountDomainEntity DepositToOwnAccount(int accountId, decimal amount)
        {
            AccountTransactionDomainEntity transaction = null;
            AccountDomainEntity account = null;
            try
            {
                account = GetAccountById(accountId);
                var transactionType = coreContext.Query<ITransactionTypeRepository>()
                    .GetByKey(TransactionTypeEnum.AccountItself.ToString());

                var transactionStatus = coreContext.Query<ITransactionStatusRepository>()
                    .GetByKey(TransactionStatusEnum.InProgress.ToString());

                transaction = coreContext.New<AccountTransactionDomainEntity>()
                    .With(account, account, amount, transactionType, transactionStatus, account);

                transaction.Insert();
                account.Deposit(amount);

                transaction.TransactionStatus = coreContext
                    .Query<ITransactionStatusRepository>()
                    .GetByKey(TransactionStatusEnum.Succeeded.ToString());
                transaction.Save();
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    transaction.TransactionStatus = coreContext
                    .Query<ITransactionStatusRepository>()
                    .GetByKey(TransactionStatusEnum.Failed.ToString());
                    transaction.Save();
                }
            }
            
            return account;
        }

        public void TransferMoney(
            AccountDomainEntity fromAccount, 
            AccountDomainEntity toAccount, 
            decimal amount, 
            TransactionTypeEnum transactionTypeEnum,
            ITransactionOwner transactionOwner)
        {
            AccountTransactionDomainEntity transaction = null;
            bool isWithdrawOk = false;
            bool isDepositOk = false;

            try
            {
                var transactionType = coreContext
                    .Query<ITransactionTypeRepository>()
                    .GetByKey(transactionTypeEnum.ToString());

                var transactionStatus = coreContext
                    .Query<ITransactionStatusRepository>()
                    .GetByKey(TransactionStatusEnum.InProgress.ToString());

                transaction = coreContext
                    .New<AccountTransactionDomainEntity>()
                    .With(fromAccount, toAccount, amount, transactionType, transactionStatus, transactionOwner);

                isWithdrawOk = fromAccount.Withdraw(amount, false);
                isDepositOk = toAccount.Deposit(amount, false);


            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    if (isWithdrawOk && isDepositOk == false)
                    {

                    }
                }
            }
        }
        #endregion

        #endregion

        #region AccountTransaction

        public List<AccountTransactionDomainEntity> GetLastAccountTransactionListByAccount(int accountId, int itemCount)
        {
            var account = GetAccountById(accountId);
            return coreContext.Query<IAccountTransactionRepository>().GetLastListByAccount(account, itemCount);
        }

        public List<AccountTransactionDomainEntity> GetLastIncomingAccountTransactionListByAccount(int accountId, int itemCount)
        {
            var account = GetAccountById(accountId);
            return coreContext.Query<IAccountTransactionRepository>().GetLastIncomingListByAccount(account, itemCount);
        }

        public List<AccountTransactionDomainEntity> GetLastOutgoingAccountTransactionListByAccount(int accountId, int itemCount)
        {
            var account = GetAccountById(accountId);
            return coreContext.Query<IAccountTransactionRepository>().GetLastOutgoingListByAccount(account, itemCount);
        }

        public List<AccountTransactionDomainEntity> GetLastDateRangeAccountTransactionListByAccount(int accountId, DateTime startDate, DateTime endDate)
        {
            var account = GetAccountById(accountId);
            return coreContext.Query<IAccountTransactionRepository>().GetLastDateRangeListByAccount(account, startDate, endDate);
        }

        public List<AccountTransactionDomainEntity> GetLastIncomingDateRangeAccountTransactionListByAccount(int accountId, DateTime startDate, DateTime endDate)
        {
            var account = GetAccountById(accountId);
            return coreContext.Query<IAccountTransactionRepository>().GetLastIncomingDateRangeListByAccount(account, startDate, endDate);
        }

        public List<AccountTransactionDomainEntity> GetLastOutgoingDateRangeAccountTransactionListByAccount(int accountId, DateTime startDate, DateTime endDate)
        {
            var account = GetAccountById(accountId);
            return coreContext.Query<IAccountTransactionRepository>().GetLastOutgoingDateRangeListByAccount(account, startDate, endDate);
        }
        #endregion

        #region Person

        #region privates

        #endregion

        #region publics
        public PersonDomainEntity CreatePerson(string firstName, string lastName, string emailAddress, string identityNumber)
        {
            var person = coreContext.New<PersonDomainEntity>()
                .With(firstName, lastName, emailAddress, identityNumber);

            person.Insert();
            return person;
        }

        public PersonDomainEntity ChangePersonLastName(int personId, string lastName)
        {
            var person = coreContext.Query<IPersonRepository>().GetById(personId);
            person.SetLastname(lastName);
            return person;
        }

        public PersonDomainEntity ChangePersonEmailAddress(int personId, string emailAddress)
        {
            var person = coreContext.Query<IPersonRepository>().GetById(personId);
            person.SetEmail(emailAddress);
            return person;
        }

        public PersonDomainEntity GetPersonById(int personId)
        {
            return coreContext.Query<IPersonRepository>().GetById(personId);
        }
        #endregion

        #endregion

        #region Company

        #region privates
        private CompanyDomainEntity CreateCompany(
            string companyName, 
            PersonDomainEntity responsablePerson, 
            string address, 
            string phoneNumber,
            string taxNumber)
        {
            var company = coreContext.New<CompanyDomainEntity>()
                .With(companyName, responsablePerson, address, phoneNumber, taxNumber);

            company.Insert();
            return company;
        }

        private CompanyDomainEntity GetCompanyByResponsablePerson(PersonDomainEntity responsablePerson)
        {
            return coreContext.Query<ICompanyRepository>().GetByResponsablePerson(responsablePerson);
        }
        #endregion

        #region publics
        public CompanyDomainEntity CreateCompany(
            string companyName, 
            int responsablePersonId, 
            string address, 
            string phoneNumber,
            string taxNumber)
        {
            var person = GetPersonById(responsablePersonId);
            return CreateCompany(companyName, person, address, phoneNumber, taxNumber);
        }

        public CompanyDomainEntity GetCompanyById(int companyId)
        {
            return coreContext.Query<ICompanyRepository>().GetById(companyId);
        }

        public CompanyDomainEntity GetCompanyByResponsableId(int responsablePersonId)
        {
            var person = GetPersonById(responsablePersonId);
            return GetCompanyByResponsablePerson(person);
        }

        public CompanyDomainEntity ChangeCompanyAddress(int companyId, string address)
        {
            var company = GetCompanyById(companyId);
            company.SetAddress(address);
            return company;
        }

        public CompanyDomainEntity ChangeCompanyPhoneNumber(int companyId, string phoneNumber)
        {
            var company = GetCompanyById(companyId);
            company.SetPhoneNumber(phoneNumber);
            return company;
        }

        public CompanyDomainEntity GetCompanyByTaxNumber(string taxNumber)
        {
            return coreContext.Query<ICompanyRepository>().GetByTaxNumber(taxNumber);
        }
        #endregion

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

        #endregion
    }
}
