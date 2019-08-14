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
        private readonly ICoreContext coreContext;
        private readonly IAccountTransactionRepository accountTransactionRepository;

        public AccountTransactionDomainEntity(
            ICoreContext coreContext,
            IAccountTransactionRepository accountTransactionRepository)
        {
            this.coreContext = coreContext;
            this.accountTransactionRepository = accountTransactionRepository;
        }
        #endregion

        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public ITransactionOwner FromTransactionOwner { get; set; }
        public ITransactionOwner ToTransactionOwner { get; set; }
        public ITransactionOwner TransactionOwner { get; set; }

        int IDomainEntity.Id => TransactionId;
        public ITransactionTypeInfo TransactionType { get; set; }
        public ITransactionStatusInfo TransactionOrderStatus { get; set; }

        public AccountTransactionDomainEntity With(
            ITransactionOwner from,
            ITransactionOwner to, 
            decimal amount,
            ITransactionTypeInfo transactionType,
            ITransactionStatusInfo transactionStatus,
            ITransactionOwner transactionOwner)
        {
            Amount = amount;
            TransactionType = transactionType;
            TransactionOrderStatus = transactionStatus;
            FromTransactionOwner = from;
            ToTransactionOwner = to;
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

        public TransactionDetailDomainEntity CreateTransactionDetail(TransactionDirection transactionDirection)
        {
            string description = ApiUtils.GenerateTransactionDescription(
                transactionDirection, FromTransactionOwner, ToTransactionOwner, Amount);

            if (transactionDirection == TransactionDirection.In)
            {
                return coreContext.New<TransactionDetailDomainEntity>()
                    .With(description, TransactionDate, this, ToTransactionOwner, transactionDirection);
            }
            else
            {
                return coreContext.New<TransactionDetailDomainEntity>()
                    .With(description, TransactionDate, this, FromTransactionOwner, transactionDirection);
            }
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

        private ITransactionOwner GetTransactionOwner(int? ownerType, int? ownerId)
        {
            if (ownerType == null || ownerId == null)
                return null;

            ITransactionOwner transactionOwner = null;
            if (ownerType == TransactionOwnerType.Account.ToInt())
            {
                transactionOwner = accountRepository.GetById(ownerId.Value);
            }
            else if (ownerType == TransactionOwnerType.TransactionOrder.ToInt())
            {
                transactionOwner = transactionOrderRepository.GetById(ownerId.Value);
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
            domainEntity.TransactionDate = dbEntity.TransactionDate;
            domainEntity.TransactionId = dbEntity.TransactionId;
            domainEntity.FromTransactionOwner = GetTransactionOwner(dbEntity.FromOwnerType, dbEntity.FromOwnerId);
            domainEntity.ToTransactionOwner = GetTransactionOwner(dbEntity.ToOwnerType, dbEntity.ToOwnerId);
            domainEntity.TransactionOrderStatus = transactionStatusRepository.GetById(dbEntity.TransactionStatusId);
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
            dbEntity.Amount = domainEntity.Amount;
            dbEntity.TransactionDate = domainEntity.TransactionDate;
            dbEntity.TransactionStatusId = domainEntity.TransactionOrderStatus.StatusId;
            dbEntity.TransactionTypeId = domainEntity.TransactionType.TypeId;
            dbEntity.FromOwnerType = domainEntity.FromTransactionOwner?.OwnerType.ToInt();
            dbEntity.FromOwnerId = domainEntity.FromTransactionOwner?.OwnerId;
            dbEntity.ToOwnerType = domainEntity.ToTransactionOwner?.OwnerType.ToInt();
            dbEntity.ToOwnerId = domainEntity.ToTransactionOwner?.OwnerId;
            dbEntity.OwnerType = domainEntity.TransactionOwner?.OwnerType.ToInt();
            dbEntity.OwnerId = domainEntity.TransactionOwner?.OwnerId;
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


        
        public List<AccountTransactionDomainEntity> GetLastListByTransactionOwner(
            ITransactionOwner transactionOwner,
            int itemCount = 15)
        {
            return GetLastItemCountListBy(
                at =>
                    ((at.FromOwnerType == transactionOwner.OwnerType.ToInt() && at.FromOwnerId == transactionOwner.OwnerId) ||
                    (at.ToOwnerType == transactionOwner.OwnerType.ToInt() && at.ToOwnerId == transactionOwner.OwnerId)),
                at =>
                    at.TransactionDate,
                itemCount)
                .ToList();
        }

        
        public List<AccountTransactionDomainEntity> GetLastIncomingListByTransactionOwner(
            ITransactionOwner transactionOwner,
            int itemCount)
        {
            return GetLastItemCountListBy(
                at =>
                    at.ToOwnerType == transactionOwner.OwnerType.ToInt() && at.ToOwnerId == transactionOwner.OwnerId,
                at =>
                    at.TransactionDate,
                itemCount)
                .ToList();
        }
        
        public List<AccountTransactionDomainEntity> GetLastDateRangeListByTransactionOwner(
            ITransactionOwner transactionOwner,
            DateTime startDate,
            DateTime endDate)
        {
            return GetOrderedDescListBy(
                at =>
                    ((at.FromOwnerType == transactionOwner.OwnerType.ToInt() && at.FromOwnerId == transactionOwner.OwnerId) ||
                    (at.ToOwnerType == transactionOwner.OwnerType.ToInt() && at.ToOwnerId == transactionOwner.OwnerId)),
                at =>
                    at.TransactionDate)
                .ToList();
        }
        
        public List<AccountTransactionDomainEntity> GetLastIncomingDateRangeListByTransactionOwner(
            ITransactionOwner transactionOwner,
            DateTime startDate,
            DateTime endDate)
        {
            return GetOrderedDescListBy(
                at =>
                    at.ToOwnerType == transactionOwner.OwnerType.ToInt() && at.ToOwnerId == transactionOwner.OwnerId,
                at =>
                    at.TransactionDate)
                .ToList();
        }
        
        public List<AccountTransactionDomainEntity> GetLastOutgoingDateRangeListByTransactionOwner(
            ITransactionOwner transactionOwner,
            DateTime starDate,
            DateTime endDate)
        {
            return GetOrderedDescListBy(
                at =>
                    at.FromOwnerType == transactionOwner.OwnerType.ToInt() && at.FromOwnerId == transactionOwner.OwnerId,
                at =>
                    at.TransactionDate)
                .ToList();
        }
        
        public List<AccountTransactionDomainEntity> GetLastOutgoingListByTransactionOwner(ITransactionOwner transactionOwner, int itemCount)
        {
            return GetLastItemCountListBy(
                at =>
                    at.FromOwnerType == transactionOwner.OwnerType.ToInt() && at.FromOwnerId == transactionOwner.OwnerId,
                at =>
                    at.TransactionDate,
                itemCount)
                .ToList();
        }
    }

    public interface IAccountTransactionRepository : IRepository<AccountTransactionDomainEntity>
    {
        /// <summary>
        /// Returns the ordered by TransactionDate descendingly 
        /// AccountTransaction list related with the given ITransactionOwner and has given item count
        /// </summary>
        /// <param name="transactionOwner">related transactionOwner</param>
        /// <param name="itemCount">represents the AccountTransaction count will be retrieved</param>
        /// <returns></returns>
        List<AccountTransactionDomainEntity> GetLastListByTransactionOwner(ITransactionOwner transactionOwner, int itemCount = 15);

        /// <summary>
        /// Returns the ordered by TransactionDate descendingly incoming way
        /// AccountTransaction list related with the given ITransactionOwner and has given item count
        /// </summary>
        /// <param name="transactionOwner">TransactionOwner with transaction entry</param>
        /// <param name="itemCount">represents the AccountTransaction count will be retrieved</param>
        /// <returns></returns>
        List<AccountTransactionDomainEntity> GetLastIncomingListByTransactionOwner(ITransactionOwner transactionOwner, int itemCount);

        /// <summary>
        /// Returns the ordered by TransactionDate descendingly outgoing way
        /// AccountTransaction list related with the given ITransactionOwner and has given item count
        /// </summary>
        /// <param name="transactionOwner">TransactionOwner with transaction outgoing</param>
        /// <param name="itemCount">represents the AccountTransaction count will be retrieved</param>
        /// <returns></returns>
        List<AccountTransactionDomainEntity> GetLastOutgoingListByTransactionOwner(ITransactionOwner transactionOwner, int itemCount);

        /// <summary>
        /// Returns the limited to given date ranges and ordered by TransactionDate descendingly
        /// AccountTransaction list related with the given ITransactionOwner
        /// </summary>
        /// <param name="transactionOwner">related ITransactionOwner</param>
        /// <param name="startDate">start date limit of transaction</param>
        /// <param name="endDate">end date limit of transaction</param>
        /// <returns></returns>
        List<AccountTransactionDomainEntity> GetLastDateRangeListByTransactionOwner(ITransactionOwner transactionOwner, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Returns the limited to given date ranges and ordered by TransactionDate descendingly incoming way
        /// AccountTransaction list related with the given ITransactionOwner
        /// </summary>
        /// <param name="transactionOwner">ITransactionOwner with transaction entry</param>
        /// <param name="startDate">start date limit of transaction</param>
        /// <param name="endDate">end date limit of transaction</param>
        /// <returns></returns>
        List<AccountTransactionDomainEntity> GetLastIncomingDateRangeListByTransactionOwner(ITransactionOwner transactionOwner, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Returns the limited to given date ranges and ordered by TransactionDate descendingly outgoing way
        /// AccountTransaction list related with the given ITransactionOwner
        /// </summary>
        /// <param name="transactionOwner">ITransactionOwner with transaction outgoing</param>
        /// <param name="starDate">start date limit of transaction</param>
        /// <param name="endDate">end date limit of transaction</param>
        /// <returns></returns>
        List<AccountTransactionDomainEntity> GetLastOutgoingDateRangeListByTransactionOwner(ITransactionOwner transactionOwner, DateTime startDate, DateTime endDate);
    }
}
