using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Repository;
using AydoganFBank.Common;
using AydoganFBank.Common.Builders;
using AydoganFBank.Common.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AydoganFBank.AccountManagement.Domain
{
    public class AccountTransactionDomainEntity : IDomainEntity, ITransactionTypeOwner, ITransactionStatusOwner
    {
        #region IoC
        private readonly IAccountTransactionRepository accountTransactionRepository;

        public AccountTransactionDomainEntity(IAccountTransactionRepository accountTransactionRepository)
        {
            this.accountTransactionRepository = accountTransactionRepository;
        }
        #endregion

        public int TransactionId { get; set; }
        public AccountDomainEntity FromAccount { get; set; }
        public AccountDomainEntity ToAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public ITransactionOwner TransactionOwner { get; set; }

        int IDomainEntity.Id => TransactionId;
        public ITransactionTypeInfo TransactionType { get; set; }
        public ITransactionStatusInfo TransactionStatus { get; set; }

        public AccountTransactionDomainEntity With(
            AccountDomainEntity fromAccount, 
            AccountDomainEntity toAccount, 
            decimal amount,
            ITransactionTypeInfo transactionType,
            ITransactionStatusInfo transactionStatus,
            ITransactionOwner transactionOwner)
        {
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Amount = amount;
            TransactionType = transactionType;
            TransactionStatus = transactionStatus;
            TransactionOwner = transactionOwner;
            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            TransactionDate = DateTime.Now;
            accountTransactionRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            accountTransactionRepository.UpdateEntity(this);
        }

        public ITransactionStatusInfo DoTransaction()
        {
            throw new NotImplementedException();
        } 
    }

    public class AccountTransactionRepository : 
        OrderedQueryRepository<AccountTransactionDomainEntity, AccountTransaction>,
        IAccountTransactionRepository,
        IDomainObjectBuilderRepository<AccountTransactionDomainEntity, AccountTransaction>
    {
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionStatusRepository transactionStatusRepository;
        private readonly ITransactionTypeRepository transactionTypeRepository;
        private readonly ITransactionOrderRepository transactionOrderRepository;
        
        public AccountTransactionRepository(
            ICoreContext coreContext,
            ITransactionStatusRepository transactionStatusRepository,
            ITransactionTypeRepository transactionTypeRepository,
            IAccountRepository accountRepository,
            ITransactionOrderRepository transactionOrderRepository) : base(coreContext, null, null)
        {
            this.transactionStatusRepository = transactionStatusRepository;
            this.transactionTypeRepository = transactionTypeRepository;
            this.accountRepository = accountRepository;
            this.transactionOrderRepository = transactionOrderRepository;
        }

        private ITransactionOwner GetTransactionOwner(AccountTransaction accountTransaction)
        {
            ITransactionOwner transactionOwner = null;
            if (accountTransaction.OwnerType == TransactionOwnerType.Account.ToInt())
            {
                transactionOwner = accountRepository.GetById(accountTransaction.OwnerId);
            }
            else if (accountTransaction.OwnerType == TransactionOwnerType.TransactionOrder.ToInt())
            {
                transactionOwner = transactionOrderRepository.GetById(accountTransaction.OwnerId);
            }
            return transactionOwner;
        }

        #region Mapping overrides
        public override AccountTransactionDomainEntity MapToDomainObject(AccountTransaction dbEntity)
        {
            if (dbEntity == null)
                return null;

            var domainEntity = coreContext.New<AccountTransactionDomainEntity>();
            MapToDomainObject(domainEntity, dbEntity);
            return domainEntity;
        }

        public override void MapToDomainObject(AccountTransactionDomainEntity domainEntity, AccountTransaction dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.Amount = dbEntity.Amount;
            domainEntity.FromAccount = accountRepository.GetById(dbEntity.FromAccountId);
            domainEntity.ToAccount = accountRepository.GetById(dbEntity.ToAccountId);
            domainEntity.TransactionDate = dbEntity.TransactionDate;
            domainEntity.TransactionId = dbEntity.TransactionId;
            domainEntity.TransactionOwner = GetTransactionOwner(dbEntity);
            domainEntity.TransactionStatus = transactionStatusRepository.GetById(dbEntity.TransactionStatusId);
            domainEntity.TransactionType = transactionTypeRepository.GetById(dbEntity.TransactionTypeId);
        }

        public override IEnumerable<AccountTransactionDomainEntity> MapToDomainObjectList(IEnumerable<AccountTransaction> dbEntities)
        {
            List<AccountTransactionDomainEntity> domainEntities = new List<AccountTransactionDomainEntity>();
            foreach (var dbEntity in dbEntities)
            {
                domainEntities.Add(MapToDomainObject(dbEntity));
            }
            return domainEntities;
        }

        public override void MapToDbEntity(AccountTransactionDomainEntity domainEntity, AccountTransaction dbEntity)
        {
            dbEntity.FromAccountId = domainEntity.FromAccount.AccountId;
            dbEntity.ToAccountId = domainEntity.ToAccount.AccountId;
            dbEntity.Amount = domainEntity.Amount;
            dbEntity.OwnerId = domainEntity.TransactionOwner.OwnerId;
            dbEntity.OwnerType = (int)domainEntity.TransactionOwner.OwnerType;
            dbEntity.TransactionDate = domainEntity.TransactionDate;
            dbEntity.TransactionStatusId = domainEntity.TransactionStatus.StatusId;
            dbEntity.TransactionTypeId = domainEntity.TransactionType.TypeId;
        }
        #endregion

        public override AccountTransactionDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override AccountTransaction GetDbEntityById(int id)
        {
            return dbContext.AccountTransaction.FirstOrDefault(at => at.TransactionId == id);
        }

        /// <summary>
        /// Returns the ordered by TransactionDate descendingly 
        /// AccountTransaction list related with the given account and has given item count
        /// </summary>
        /// <param name="account">related Account</param>
        /// <param name="itemCount">represents the AccountTransaction count will be retrieved</param>
        /// <returns></returns>
        public List<AccountTransactionDomainEntity> GetLastListByAccount(
            AccountDomainEntity account, 
            int itemCount = 15)
        {
            return GetLastItemCountListBy(
                at => 
                    (at.FromAccountId == account.AccountId || at.ToAccountId == account.AccountId),
                at => 
                    at.TransactionDate, 
                itemCount)
                .ToList();
        }

        /// <summary>
        /// Returns the ordered by TransactionDate descendingly incoming way
        /// AccountTransaction list related with the given account and has given item count
        /// </summary>
        /// <param name="account">Account with transaction entry</param>
        /// <param name="itemCount">represents the AccountTransaction count will be retrieved</param>
        /// <returns></returns>
        public List<AccountTransactionDomainEntity> GetLastIncomingListByAccount(
            AccountDomainEntity account, 
            int itemCount)
        {
            return GetLastItemCountListBy(
                at => 
                    at.ToAccountId == account.AccountId,
                at => 
                    at.TransactionDate,
                itemCount)
                .ToList();
        }

        /// <summary>
        /// Returns the ordered by TransactionDate descendingly outgoing way
        /// AccountTransaction list related with the given account and has given item count
        /// </summary>
        /// <param name="account">Account with transaction outgoing</param>
        /// <param name="itemCount">represents the AccountTransaction count will be retrieved</param>
        /// <returns></returns>
        public List<AccountTransactionDomainEntity> GetLastOutgoingListByAccount(
            AccountDomainEntity account, 
            int itemCount)
        {
            return GetLastItemCountListBy(
                at => 
                    at.FromAccountId == account.AccountId,
                at => 
                    at.TransactionDate,
                itemCount)
                .ToList();
        }

        /// <summary>
        /// Returns the limited to given date ranges and ordered by TransactionDate descendingly
        /// AccountTransaction list related with the given account
        /// </summary>
        /// <param name="account">related Account</param>
        /// <param name="startDate">start date limit of transaction</param>
        /// <param name="endDate">end date limit of transaction</param>
        /// <returns></returns>
        public List<AccountTransactionDomainEntity> GetLastDateRangeListByAccount(
            AccountDomainEntity account, 
            DateTime startDate, 
            DateTime endDate)
        {
            return GetOrderedDescListBy(
                at => 
                    (at.FromAccountId == account.AccountId || at.ToAccountId == account.AccountId) 
                    && at.TransactionDate >= startDate && at.TransactionDate <= endDate,
                at => 
                    at.TransactionDate)
                .ToList();
        }

        /// <summary>
        /// Returns the limited to given date ranges and ordered by TransactionDate descendingly incoming way
        /// AccountTransaction list related with the given account
        /// </summary>
        /// <param name="account">Account with transaction entry</param>
        /// <param name="startDate">start date limit of transaction</param>
        /// <param name="endDate">end date limit of transaction</param>
        /// <returns></returns>
        public List<AccountTransactionDomainEntity> GetLastIncomingDateRangeListByAccount(
            AccountDomainEntity account,
            DateTime startDate,
            DateTime endDate)
        {
            return GetOrderedDescListBy(
                at => 
                    at.ToAccountId == account.AccountId && at.TransactionDate >= startDate && at.TransactionDate <= endDate,
                at => 
                    at.TransactionDate)
                .ToList();
        }

        /// <summary>
        /// Returns the limited to given date ranges and ordered by TransactionDate descendingly outgoing way
        /// AccountTransaction list related with the given account
        /// </summary>
        /// <param name="account">Account with transaction outgoing</param>
        /// <param name="startDate">start date limit of transaction</param>
        /// <param name="endDate">end date limit of transaction</param>
        /// <returns></returns>
        public List<AccountTransactionDomainEntity> GetLastOutgoingDateRangeListByAccount(
            AccountDomainEntity account,
            DateTime startDate,
            DateTime endDate)
        {
            return GetOrderedDescListBy(
                at => 
                    at.FromAccountId == account.AccountId && at.TransactionDate >= startDate && at.TransactionDate <= endDate,
                at => 
                    at.TransactionDate)
                .ToList();
        }
    }

    public interface IAccountTransactionRepository : IRepository<AccountTransactionDomainEntity>
    {
        List<AccountTransactionDomainEntity> GetLastListByAccount(AccountDomainEntity account, int itemCount = 15);
        List<AccountTransactionDomainEntity> GetLastIncomingListByAccount(AccountDomainEntity account, int itemCount);
        List<AccountTransactionDomainEntity> GetLastOutgoingListByAccount(AccountDomainEntity account, int itemCount);
        List<AccountTransactionDomainEntity> GetLastDateRangeListByAccount(AccountDomainEntity account, DateTime startDate, DateTime endDate);
        List<AccountTransactionDomainEntity> GetLastIncomingDateRangeListByAccount(AccountDomainEntity account, DateTime startDate, DateTime endDate);
        List<AccountTransactionDomainEntity> GetLastOutgoingDateRangeListByAccount(AccountDomainEntity account, DateTime startDate, DateTime endDate);
    }
}
