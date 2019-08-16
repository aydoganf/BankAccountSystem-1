using AydoganFBank.TransactionManagement.Api;
using AydoganFBank.Common;
using AydoganFBank.Common.IoC;
using AydoganFBank.Common.Repository;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AydoganFBank.TransactionManagement.Domain
{
    public class TransactionOrderDomainEntity : 
        IDomainEntity, 
        ITransactionOwner, 
        ITransactionTypeOwner, 
        ITransactionStatusOwner
    {
        #region IoC
        private readonly ITransactionOrderRepository transactionOrderRepository;
        private readonly ICoreContext coreContext;

        public TransactionOrderDomainEntity(
            ICoreContext coreContext,
            ITransactionOrderRepository transactionOrderRepository)
        {
            this.coreContext = coreContext;
            this.transactionOrderRepository = transactionOrderRepository;
        }
        #endregion

        public int TransactionOrderId { get; set; }
        public string OrderDesctiption { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OperationDate { get; set; }
        public AccountDomainEntity FromAccount { get; set; }
        public AccountDomainEntity ToAccount { get; set; }
        public decimal Amount { get; set; }

        public ITransactionTypeInfo TransactionType { get; set; }
        public ITransactionStatusInfo TransactionStatus { get; set; }

        int IDomainEntity.Id => TransactionOrderId;
        int ITransactionOwner.OwnerId => TransactionOrderId;
        string ITransactionOwner.TransactionDetailDisplayName => string.Format("{0} - Transaction order", OperationDate.ToFormattedString());
        string ITransactionOwner.AssetsUnit => ((ITransactionOwner)ToAccount).AssetsUnit;
        TransactionOwnerType ITransactionOwner.OwnerType => TransactionOwnerType.TransactionOrder;


        public TransactionOrderDomainEntity With(
            ITransactionTypeInfo transactionType, 
            string orderDescription,
            DateTime operationDate,
            AccountDomainEntity fromAccount,
            AccountDomainEntity toAccount,
            decimal amount,
            ITransactionStatusInfo transactionOrderStatus)
        {
            TransactionType = transactionType;
            OrderDesctiption = orderDescription;
            CreateDate = DateTime.Now;
            OperationDate = operationDate;
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Amount = amount;
            TransactionStatus = transactionOrderStatus;
            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            transactionOrderRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            transactionOrderRepository.UpdateEntity(this);
        }

        public void SetStatus(TransactionStatusEnum transactionStatus)
        {
            TransactionStatus = coreContext
                .Query<ITransactionStatusRepository>()
                .GetByKey(transactionStatus.ToString());
        }
    }

    public class TransactionOrderRepository :
        Repository<TransactionOrderDomainEntity, TransactionOrder>,
        ITransactionOrderRepository,
        IDomainObjectBuilderRepository<TransactionOrderDomainEntity, TransactionOrder>
    {
        public TransactionOrderRepository(ICoreContext coreContext) 
            : base(coreContext, null, null)
        {
        }

        #region Mapping overrides
        public override TransactionOrderDomainEntity MapToDomainObject(TransactionOrder dbEntity)
        {
            if (dbEntity == null)
                return null;

            var domainEntity = coreContext.New<TransactionOrderDomainEntity>();
            MapToDomainObject(domainEntity, dbEntity);
            return domainEntity;
        }

        public override void MapToDomainObject(TransactionOrderDomainEntity domainEntity, TransactionOrder dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.Amount = dbEntity.Amount;
            domainEntity.CreateDate = dbEntity.CreateDate;
            domainEntity.FromAccount = coreContext.Query<IAccountRepository>().GetById(dbEntity.FromAccountId);
            domainEntity.OperationDate = dbEntity.OperationDate;
            domainEntity.OrderDesctiption = dbEntity.OrderDescription;
            domainEntity.ToAccount = coreContext.Query<IAccountRepository>().GetById(dbEntity.ToAccountId);
            domainEntity.TransactionOrderId = dbEntity.TransactionOrderId;
            domainEntity.TransactionType = coreContext.Query<ITransactionTypeRepository>().GetById(dbEntity.TransactionTypeId);
            domainEntity.TransactionStatus = coreContext.Query<ITransactionStatusRepository>().GetById(dbEntity.TransactionOrderStatusId);
        }

        public override IEnumerable<TransactionOrderDomainEntity> MapToDomainObjectList(IEnumerable<TransactionOrder> dbEntities)
        {
            List<TransactionOrderDomainEntity> domainEntities = new List<TransactionOrderDomainEntity>();
            foreach (var dbEntity in dbEntities)
            {
                domainEntities.Add(MapToDomainObject(dbEntity));
            }
            return domainEntities;
        }

        public override void MapToDbEntity(TransactionOrderDomainEntity domainEntity, TransactionOrder dbEntity)
        {
            dbEntity.Amount = domainEntity.Amount;
            dbEntity.CreateDate = domainEntity.CreateDate;
            dbEntity.FromAccountId = domainEntity.FromAccount.AccountId;
            dbEntity.OperationDate = domainEntity.OperationDate;
            dbEntity.OrderDescription = domainEntity.OrderDesctiption;
            dbEntity.ToAccountId = domainEntity.ToAccount.AccountId;
            dbEntity.TransactionTypeId = domainEntity.TransactionType.TypeId;
            dbEntity.TransactionOrderStatusId = domainEntity.TransactionStatus.StatusId;
        }
        #endregion

        public override TransactionOrderDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override TransactionOrder GetDbEntityById(int id)
        {
            return dbContext.TransactionOrder.FirstOrDefault(to => to.TransactionOrderId == id);
        }

        public List<TransactionOrderDomainEntity> GetListByFromAccount(AccountDomainEntity fromAccount)
        {
            List<int> statusList = new List<int>()
            {
                TransactionStatusEnum.Created.ToInt(),
                TransactionStatusEnum.Pending.ToInt()
            };

            return GetListBy(
                to =>
                    to.FromAccountId == fromAccount.AccountId && statusList.Contains(to.TransactionOrderStatusId))
                .ToList();
        }

        public List<TransactionOrderDomainEntity> GetListByOperationDate(DateTime operationDate)
        {
            return GetListBy(
                to =>
                    to.OperationDate == operationDate)
                .ToList();
        }

        public List<TransactionOrderDomainEntity> GetUncompletedListByOperationDate(DateTime operationDate)
        {
            List<int> statusList = new List<int>()
            {
                TransactionStatusEnum.Created.ToInt(),
                TransactionStatusEnum.Pending.ToInt()
            };

            return GetListBy(
                to =>
                    to.OperationDate == operationDate && statusList.Contains(to.TransactionOrderStatusId))
                .ToList();
        }

    }

    public interface ITransactionOrderRepository : IRepository<TransactionOrderDomainEntity>
    {
        /// <summary>
        /// Gets the Created or Pending Transaction orders related with given account
        /// </summary>
        /// <param name="fromAccount">related Account</param>
        /// <returns></returns>
        List<TransactionOrderDomainEntity> GetListByFromAccount(AccountDomainEntity fromAccount);

        /// <summary>
        /// Gets the all TransactionOrders 
        /// which will be operated on given date
        /// </summary>
        /// <param name="operationDate"></param>
        /// <returns></returns>
        List<TransactionOrderDomainEntity> GetListByOperationDate(DateTime operationDate);

        /// <summary>
        /// Gets the Created or Pending state TransactionOrders 
        /// which will be operated on given date.
        /// </summary>
        /// <param name="operationDate">operation date</param>
        /// <returns></returns>
        List<TransactionOrderDomainEntity> GetUncompletedListByOperationDate(DateTime operationDate);
    }
}
