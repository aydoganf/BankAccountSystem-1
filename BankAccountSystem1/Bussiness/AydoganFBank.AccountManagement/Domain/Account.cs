using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Context.Exception;
using AydoganFBank.Context.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
{

    public class AccountDomainEntity : IDomainEntity, ITransactionOwner, ITransactionDetailOwner, ICreditCardOwner, IAccountInfo
    {
        #region IoC
        private readonly ICoreContext coreContext;
        private readonly IAccountRepository accountRepository;

        public AccountDomainEntity(
            ICoreContext coreContext,
            IAccountRepository accountRepository)
        {
            this.coreContext = coreContext;
            this.accountRepository = accountRepository;
        }
        #endregion

        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public AccountTypeDomainEntity AccountType { get; set; }
        public decimal Balance { get; set; }
        public IAccountOwner AccountOwner { get; set; }

        int IDomainEntity.Id => AccountId;

        int ITransactionOwner.OwnerId => AccountId;
        TransactionOwnerType ITransactionOwner.OwnerType => TransactionOwnerType.Account;
        string ITransactionOwner.TransactionDetailDisplayName => AccountOwner.DisplayName;
        string ITransactionOwner.AssetsUnit => AccountType.AssetsUnit;

        int ICreditCardOwner.OwnerId => AccountId;
        CreditCardOwnerType ICreditCardOwner.CreditCardOwnerType => CreditCardOwnerType.Account;
        string ICreditCardOwner.AssetsUnit => AccountType.AssetsUnit;
        string ICreditCardOwner.DisplayText => AccountOwner.DisplayName;

        int ITransactionDetailOwner.OwnerId => AccountId;
        TransactionDetailOwnerType ITransactionDetailOwner.OwnerType => TransactionDetailOwnerType.Account;

        public AccountDomainEntity With(
            AccountTypeDomainEntity accountType, 
            IAccountOwner accountOwner,
            string accountNumber)
        {
            AccountType = accountType ?? throw new CommonException.RequiredParameterMissingException(nameof(accountType));
            AccountOwner = accountOwner ?? throw new CommonException.RequiredParameterMissingException(nameof(accountType));
            AccountNumber = string.IsNullOrWhiteSpace(accountNumber) ? 
                throw new CommonException.RequiredParameterMissingException(nameof(accountNumber)) : accountNumber;

            accountRepository.InsertEntity(this);
            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            accountRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            accountRepository.UpdateEntity(this);
        }

        public void Flush()
        {
            accountRepository.FlushEntity(this);
        }

        public void Delete()
        {
            accountRepository.DeleteEntity(this);
        }

        internal bool Deposit(decimal amount, bool forceToUpdateDb = true)
        {
            if (amount <= 0)
                throw new AccountManagementException.DepositAmountCanNotBeZeroOrNegativeException(string.Format("{0} = {1}", nameof(amount), amount));

            Balance += amount;
            if(forceToUpdateDb)
                Save();

            accountRepository.FlushEntity(this);
            return true;
        }

        internal bool Withdraw(decimal amount, bool forceToUpdateDb = true)
        {
            if (amount <= 0)
                throw new AccountManagementException.WithdrawAmountCanNotBeZeroOrNegativeException(string.Format("{0} = {1}", nameof(amount), amount));

            if (amount > Balance)
                throw new AccountManagementException.AccountHasNotEnoughBalanceForWithdrawAmount(string.Format("{0} = {1}", nameof(amount), amount));

            Balance -= amount;
            if (forceToUpdateDb == false)
                Save();

            accountRepository.FlushEntity(this);
            return true;
        }

        public List<AccountTransactionDomainEntity> GetLastIncomingDateRangeAccountTransactionList(
            DateTime startDate,
            DateTime endDate)
        {
            return coreContext
                .Query<IAccountTransactionRepository>()
                .GetLastIncomingDateRangeListByTransactionOwner(this, startDate, endDate);
        }

        public List<AccountTransactionDomainEntity> GetLastOutgoingDateRangeAccountTransactionList(
            DateTime startDate,
            DateTime endDate)
        {
            return coreContext
                .Query<IAccountTransactionRepository>()
                .GetLastOutgoingDateRangeListByTransactionOwner(this, startDate, endDate);
        }

        public List<AccountTransactionDomainEntity> GetLastDateRangeAccountTransactionList(
            DateTime startDate,
            DateTime endDate)
        {
            return coreContext
                .Query<IAccountTransactionRepository>()
                .GetLastDateRangeListByTransactionOwner(this, startDate, endDate);
        }

        public List<TransactionDetailDomainEntity> GetLastTransactionDetailDateRangeList(DateTime startDate, DateTime endDate)
        {
            return coreContext.Query<ITransactionDetailRepository>()
                .GetLastDateRangeListByTransactionDetailOwner(this, startDate, endDate);
        }

        public List<TransactionDetailDomainEntity> GetTransactionDetailDateRangeList(DateTime startDate, DateTime endDate)
        {
            return coreContext.Query<ITransactionDetailRepository>()
                .GetDateRangeListByTransactionDetailOwner(this, startDate, endDate);
        }

        public List<TransactionDetailDomainEntity> GetLastTransactionDetailDateRangeAndTransactionDirectionList(
            TransactionDirection transactionDirection, DateTime startDate, DateTime endDate)
        {
            return coreContext.Query<ITransactionDetailRepository>()
                .GetLastDateRangeAndTransactionDirectionListByTransactionDetailOwner(this, transactionDirection, startDate, endDate);
        }

        #region Api
        int IAccountInfo.Id => AccountId;
        string IAccountInfo.AccountNumber => AccountNumber;
        IAccountTypeInfo IAccountInfo.AccountType => AccountType;
        decimal IAccountInfo.Balance => Balance;
        IAccountOwner IAccountInfo.AccountOwner => AccountOwner;
        #endregion
    }

    public class AccountRepository : OrderedQueryRepository<AccountDomainEntity, Account>, IAccountRepository
    {
        private const int ACCOUNT_NUMBER_START = 1000000;

        public AccountRepository(ICoreContext coreContext) 
            : base(coreContext)
        {
            coreContext.Logger.Info("AccountRepository created.", coreContext.GetContainerInfo());
        }

        private IAccountOwner GetAccountOwner(Account account)
        {
            IAccountOwner accountOwner = null;
            if (account.OwnerType == AccountOwnerType.Person.ToInt())
            {
                accountOwner = coreContext.Query<IPersonRepository>().GetById(account.OwnerId) as IAccountOwner;
            }
            else if (account.OwnerType == AccountOwnerType.Company.ToInt())
            {
                accountOwner = coreContext.Query<ICompanyRepository>().GetById(account.OwnerId) as IAccountOwner;
            }
            return accountOwner;
        }

        #region Mapper overrides

        public override void MapToDomainObject(AccountDomainEntity domainEntity, Account dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.AccountId = dbEntity.AccountId;
            domainEntity.AccountNumber = dbEntity.AccountNumber;
            domainEntity.AccountType = coreContext.Query<IAccountTypeRepository>().GetById(dbEntity.AccountTypeId);
            domainEntity.Balance = dbEntity.Balance;
            domainEntity.AccountOwner = GetAccountOwner(dbEntity);
        }

        public override void MapToDbEntity(AccountDomainEntity domainEntity, Account dbEntity)
        {
            dbEntity.AccountTypeId = domainEntity.AccountType.AccountTypeId;
            dbEntity.Balance = domainEntity.Balance;
            dbEntity.OwnerId = domainEntity.AccountOwner.OwnerId;
            dbEntity.OwnerType = (int)domainEntity.AccountOwner.OwnerType;
            dbEntity.AccountNumber = domainEntity.AccountNumber;
        }

        #endregion

        public override AccountDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        public List<AccountDomainEntity> GetListByPerson(PersonDomainEntity person)
        {
            return GetListBy(
                a => 
                    a.OwnerType == AccountOwnerType.Person.ToInt() && a.OwnerId == person.PersonId
                    );
        }

        public List<AccountDomainEntity> GetLisyByCompany(CompanyDomainEntity company)
        {
            return GetListBy(
                a => 
                    a.OwnerType == AccountOwnerType.Company.ToInt() && a.OwnerId == company.CompanyId
                    );
        }

        protected override Account GetDbEntityById(int id)
        {
            return dbContext.Account.FirstOrDefault(a => a.AccountId == id);
        }

        public string GetNextAccountNumber()
        {
            AccountDomainEntity lastAccount = null;

            try
            {
                lastAccount = GetLastBy(a => a.AccountNumber);
            }
            catch (Exception)
            {
            }

            int nextAccountNumber = lastAccount != null ? Convert.ToInt32(lastAccount.AccountNumber) + 1 : ACCOUNT_NUMBER_START;
            return nextAccountNumber.ToString();
        }

        public AccountDomainEntity GetByAccountNumber(string accountNumber)
        {
            return GetFirstBy(
                a => a.AccountNumber == accountNumber);
        }

        public new List<AccountDomainEntity> GetAll()
        {
            return base.GetAll();
        }

        public List<AccountDomainEntity> By(IAccountOwner accountOwner)
        {
            int ownerType = accountOwner.OwnerType.ToInt();
            return GetListBy(
                a =>
                    a.OwnerId == accountOwner.OwnerId && a.OwnerType == ownerType);
        }

        public bool HasOwnerAccountByType(IAccountOwner accountOwner, AccountTypeDomainEntity accountType)
        {
            int ownerType = accountOwner.OwnerType.ToInt();
            return Exists(
                a =>
                    a.OwnerId == accountOwner.OwnerId && a.OwnerType == ownerType &&
                    a.AccountTypeId == accountType.AccountTypeId);
        }

        List<AccountDomainEntity> IAccountRepository.GetListByOwner(IAccountOwner accountOwner) => By(accountOwner);
    }

    public interface IAccountRepository : IRepository<AccountDomainEntity>        
    {
        List<AccountDomainEntity> GetAll();
        string GetNextAccountNumber();
        List<AccountDomainEntity> GetListByPerson(PersonDomainEntity person);
        List<AccountDomainEntity> GetLisyByCompany(CompanyDomainEntity company);
        AccountDomainEntity GetByAccountNumber(string accountNumber);
        bool HasOwnerAccountByType(IAccountOwner accountOwner, AccountTypeDomainEntity accountType);
        List<AccountDomainEntity> GetListByOwner(IAccountOwner accountOwner);
    }
}
