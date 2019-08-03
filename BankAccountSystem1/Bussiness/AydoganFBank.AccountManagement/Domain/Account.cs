using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Repository;
using AydoganFBank.Common;
using AydoganFBank.Common.Builders;
using AydoganFBank.Common.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
{

    public class AccountDomainEntity : IDomainEntity
    {
        #region IoC
        private readonly IAccountRepository accountRepository;

        public AccountDomainEntity(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        #endregion

        public int AccountId { get; set; }
        //public AccountOwnerType OwnerType { get; set; }
        //public int OwnerId { get; set; }
        public string AccountNumber { get; set; }
        public AccountTypeDomainEntity AccountType { get; set; }
        public decimal Balance { get; set; }
        public IAccountOwner AccountOwner { get; set; }

        int IDomainEntity.Id => AccountId;

        public AccountDomainEntity With(AccountTypeDomainEntity accountType, IAccountOwner accountOwner)
        {
            AccountType = accountType;
            AccountOwner = accountOwner;
            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            // calculate new account number
            string accountNumber = accountRepository.GetNextAccountNumber();
            AccountNumber = accountNumber;
            accountRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            accountRepository.UpdateEntity(this);
        }

        internal void Deposit(decimal amount)
        {
            Balance += amount;
        }

        internal void Withdraw(decimal amount)
        {
            Balance -= amount;
        }
    }

    public class AccountRepository : OrderedQueryRepository<AccountDomainEntity, Account>,
        IAccountRepository,
        IDomainObjectBuilderRepository<AccountDomainEntity, Account>
    {
        private const int ACCOUNT_NUMBER_START = 1000000;
        private readonly IDomainEntityBuilder<AccountTypeDomainEntity, AccountType> accountTypeDomainEntityBuilder;
        private readonly IPersonRepository personRepository;

        public AccountRepository(
            ICoreContext coreContext, 
            AydoganFBankDbContext dbContext,
            IDomainEntityBuilder<AccountTypeDomainEntity, AccountType> accountTypeDomainEntityBuilder,
            IPersonRepository personRepository) 
            : base(coreContext, dbContext, null, null)
        {
            this.personRepository = personRepository;
            this.accountTypeDomainEntityBuilder = accountTypeDomainEntityBuilder;
        }

        private IAccountOwner GetAccountOwner(Account account)
        {
            IAccountOwner accountOwner = null;
            if (account.OwnerType == (int)AccountOwnerType.Person)
            {
                accountOwner = personRepository.GetById(account.OwnerId);
            }
            return accountOwner;
        }

        #region Mapper overrides

        public override AccountDomainEntity MapToDomainObject(Account account)
        {
            if (account == null)
                return null;

            var domainEntity = coreContext.New<AccountDomainEntity>();
            domainEntity.AccountId = account.AccountId;
            domainEntity.AccountNumber = account.AccountNumber;
            domainEntity.AccountType = accountTypeDomainEntityBuilder.MapToDomainObject(account.AccountType);
            domainEntity.Balance = account.Balance;
            //domainEntity.OwnerId = account.OwnerId;
            //domainEntity.OwnerType = Enum.Parse<AccountOwnerType>(account.OwnerType.ToString());
            domainEntity.AccountOwner = GetAccountOwner(account);
            return domainEntity;
        }

        public override void MapToDomainObject(AccountDomainEntity domainEntity, Account dbEntity)
        {
            if (dbEntity == null)
                return;

            domainEntity.AccountId = dbEntity.AccountId;
            domainEntity.AccountNumber = dbEntity.AccountNumber;
            domainEntity.AccountType = accountTypeDomainEntityBuilder.MapToDomainObject(dbEntity.AccountType);
            domainEntity.Balance = dbEntity.Balance;
            domainEntity.AccountOwner = GetAccountOwner(dbEntity);
        }

        public override IEnumerable<AccountDomainEntity> MapToDomainObjectList(IEnumerable<Account> entities)
        {
            List<AccountDomainEntity> accounts = new List<AccountDomainEntity>();
            foreach (var entity in entities)
            {
                accounts.Add(MapToDomainObject(entity));
            }
            return accounts;
        }

        public override void MapToDbEntity(AccountDomainEntity domainEntity, Account dbEntity)
        {
            dbEntity.AccountTypeId = domainEntity.AccountType.AccountTypeId;
            dbEntity.Balance = domainEntity.Balance;
            dbEntity.OwnerId = domainEntity.AccountOwner.OwnerId;
            dbEntity.OwnerType = (int)domainEntity.AccountOwner.OwnerType;
        }

        #endregion

        public override AccountDomainEntity GetById(int id)
        {
            return base.GetFirstBy(a => a.AccountId == id);
        }

        public List<AccountDomainEntity> GetListByPerson(PersonDomainEntity person)
        {
            return GetListBy(a => a.OwnerType == (int)AccountOwnerType.Person && a.OwnerId == person.PersonId).
                ToList();
        }

        public List<AccountDomainEntity> GetLisyByCompany(CompanyDomainEntity company)
        {
            return GetListBy(a => a.OwnerType == (int)AccountOwnerType.Company && a.OwnerId == company.CompanyId).
                ToList();
        }

        protected override Account GetDbEntityById(int id)
        {
            return dbContext.Account.FirstOrDefault(a => a.AccountId == id);
        }

        public string GetNextAccountNumber()
        {
            var lastAccount = GetLastBy(a => a.AccountNumber);
            int nextAccountNumber = lastAccount != null ? Convert.ToInt32(lastAccount.AccountNumber) + 1 : ACCOUNT_NUMBER_START;
            return nextAccountNumber.ToString();
        }
    }

    public interface IAccountRepository : IRepository<AccountDomainEntity>        
    {
        string GetNextAccountNumber();
        List<AccountDomainEntity> GetListByPerson(PersonDomainEntity person);
        List<AccountDomainEntity> GetLisyByCompany(CompanyDomainEntity company);
    }
}
