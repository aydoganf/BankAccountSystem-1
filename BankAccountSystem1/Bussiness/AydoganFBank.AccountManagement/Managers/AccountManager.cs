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
        
        public AccountDomainEntity WithdrawMoneyFromOwn(int accountId, decimal amount)
        {
            AccountTransactionDomainEntity transaction = null;
            AccountDomainEntity account = null;
            bool isWithdrawOK = false;

            try
            {
                account = GetAccountById(accountId);
                var transactionType = GetTransactionTypeByKey(TransactionTypeEnum.AccountItself.ToString());

                var transactionStatus = GetTransactionStatusByKey(TransactionStatusEnum.InProgress.ToString());

                transaction = coreContext
                    .New<AccountTransactionDomainEntity>()
                    .With(account, null, amount, transactionType, transactionStatus, account);

                transaction.Insert();

                isWithdrawOK = account.Withdraw(amount);

                transaction.TransactionStatus = GetTransactionStatusByKey(TransactionStatusEnum.Succeeded.ToString());
                transaction.Save();
                var transactionDetail = transaction.CreateTransactionDetail(TransactionDirection.Out);
                transactionDetail.Insert();
            }
            catch (Exception)
            {
                if (transaction != null && isWithdrawOK == false)
                {
                    transaction.TransactionStatus = GetTransactionStatusByKey(TransactionStatusEnum.Failed.ToString());
                    transaction.Save();
                }
            }

            return account;
        }

        public AccountDomainEntity DepositToOwnAccount(int accountId, decimal amount)
        {
            AccountTransactionDomainEntity transaction = null;
            AccountDomainEntity account = null;
            bool isDepositOk = false;
            try
            {
                account = GetAccountById(accountId);
                var transactionType = GetTransactionTypeByKey(TransactionTypeEnum.AccountItself.ToString());

                var transactionStatus = GetTransactionStatusByKey(TransactionStatusEnum.InProgress.ToString());

                transaction = coreContext.New<AccountTransactionDomainEntity>()
                    .With(null, account, amount, transactionType, transactionStatus, account);

                transaction.Insert();
                isDepositOk = account.Deposit(amount);

                transaction.TransactionStatus = GetTransactionStatusByKey(TransactionStatusEnum.Succeeded.ToString());
                transaction.Save();

                var transactionDetail = transaction.CreateTransactionDetail(TransactionDirection.In);
                transactionDetail.Insert();
            }
            catch (Exception)
            {
                if (transaction != null && isDepositOk == false)
                {
                    transaction.TransactionStatus = GetTransactionStatusByKey(TransactionStatusEnum.Failed.ToString());
                    transaction.Save();
                }
            }
            
            return account;
        }

        /// <summary>
        /// Does money transfer from one account to another account with given TransactionType
        /// </summary>
        /// <param name="fromAccount"></param>
        /// <param name="toAccount"></param>
        /// <param name="amount"></param>
        /// <param name="transactionTypeEnum"></param>
        public void TransferMoney(
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
                var transactionType = GetTransactionTypeByKey(transactionTypeEnum.ToString());

                var transactionStatus = GetTransactionStatusByKey(TransactionStatusEnum.InProgress.ToString());

                transaction = coreContext.New<AccountTransactionDomainEntity>()
                    .With(fromAccount, toAccount, amount, transactionType, transactionStatus, null);
                transaction.Insert();

                isWithdrawOk = fromAccount.Withdraw(amount, false);
                isDepositOk = toAccount.Deposit(amount, false);

                transaction.TransactionStatus = GetTransactionStatusByKey(TransactionStatusEnum.Succeeded.ToString());
                transaction.Save();

                var transactionDetailIn = transaction.CreateTransactionDetail(TransactionDirection.In);
                transactionDetailIn.Insert();

                var transactionDetailOut = transaction.CreateTransactionDetail(TransactionDirection.Out);
                transactionDetailOut.Insert();
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    if (isWithdrawOk && isDepositOk == false)
                    {
                        fromAccount.Deposit(amount);
                        transaction.TransactionStatus = GetTransactionStatusByKey(TransactionStatusEnum.Failed.ToString());

                        transaction.Save();
                    }
                }
            }
        }
        #endregion

        #endregion

        #region AccountTransaction
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
            string taxNumber,
            AccountDomainEntity account)
        {
            var company = coreContext.New<CompanyDomainEntity>()
                .With(companyName, responsablePerson, address, phoneNumber, taxNumber, account);

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
            string taxNumber,
            int accountId)
        {
            var person = GetPersonById(responsablePersonId);
            var account = GetAccountById(accountId);
            return CreateCompany(companyName, person, address, phoneNumber, taxNumber, account);
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
        public List<TransactionOrderDomainEntity> GetTransactionOrderListByAccount(int accountId)
        {
            AccountDomainEntity account = GetAccountById(accountId);

            return coreContext.Query<ITransactionOrderRepository>().GetListByFromAccount(account);
        }

        public List<TransactionOrderDomainEntity> GetAllTransactionOrders(DateTime operationDate)
        {
            return coreContext.Query<ITransactionOrderRepository>().GetListByOperationDate(operationDate);
        }

        public List<TransactionOrderDomainEntity> GetUncompletedTransactionOrders(DateTime operationDate)
        {
            return coreContext.Query<ITransactionOrderRepository>().GetUncompletedListByOperationDate(operationDate);
        }

        internal void DoTransactionOrder(TransactionOrderDomainEntity transactionOrder)
        {
            try
            {
                TransferMoney(
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

        #region CreditCard
        private CreditCardDomainEntity DoCreditCardPayment(
            CreditCardDomainEntity creditCard, decimal amount, int instalmentCount, ITransactionOwner toTransactionOwner)
        {
            creditCard.DoPayment(amount);

            var transactionType = coreContext.Query<ITransactionTypeRepository>()
                    .GetByKey(TransactionTypeEnum.CreditCardPayment.ToString());

            var transactionStatus = coreContext.Query<ITransactionStatusRepository>()
                .GetByKey(TransactionStatusEnum.InProgress.ToString());

            var transaction = coreContext.New<AccountTransactionDomainEntity>()
                .With(creditCard, toTransactionOwner, amount, transactionType, transactionStatus, creditCard);

            transaction.Insert();
            var transactionDetail = transaction.CreateTransactionDetail(TransactionDirection.Out);
            transactionDetail.Insert();

            for (int instalmentIndex = 1; instalmentIndex <= instalmentCount; instalmentIndex++)
            {
                decimal instalmentAmount = amount / instalmentCount;
                string paymentDescription = string.Format("{0} - {1}{2} ({3} instalment)",
                    toTransactionOwner.TransactionDetailDisplayName, instalmentAmount, toTransactionOwner.AssetsUnit,
                    string.Format("{0}/{1}", instalmentIndex, instalmentCount));
                DateTime instalmentDate = transaction.TransactionDate.AddMonths(instalmentIndex - 1);

                var creditCardPayment = coreContext.New<CreditCardPaymentDomainEntity>()
                    .With(instalmentIndex, instalmentAmount, paymentDescription, 
                    transaction.TransactionDate, instalmentDate, transaction);
                creditCardPayment.Insert();
            }

            return creditCard;
        }

        public CreditCardDomainEntity DoCreditCardPayment(
            int creditCardId, decimal amount, int instalmentCount, ITransactionOwner toTransactionOwner)
        {
            var creditCard = coreContext.Query<ICreditCardRepository>()
                .GetById(creditCardId);

            if(creditCard == null)
            {
                //
            }

            return DoCreditCardPayment(creditCard, amount, instalmentCount, toTransactionOwner);
        }
        #endregion
    }
}
