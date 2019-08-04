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
    public class TransactionOrderDomainEntity : IDomainEntity, ITransactionOwner
    {
        #region IoC
        private readonly ITransactionOrderRepository transactionOrderRepository;

        public TransactionOrderDomainEntity(ITransactionOrderRepository transactionOrderRepository)
        {
            this.transactionOrderRepository = transactionOrderRepository;
        }
        #endregion

        public int TransactionOrderId { get; set; }
        public TransactionTypeDomainEntity TransactionType { get; set; }
        public string OrderDesctiption { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OperationDate { get; set; }
        public AccountDomainEntity FromAccount { get; set; }
        public AccountDomainEntity ToAccount { get; set; }
        public decimal Amount { get; set; }

        int IDomainEntity.Id => TransactionOrderId;

        int ITransactionOwner.OwnerId => TransactionOrderId;
        TransactionOwnerType ITransactionOwner.OwnerType => TransactionOwnerType.TransactionOrder;

        public TransactionOrderDomainEntity With(
            TransactionTypeDomainEntity transactionType, 
            string orderDescription,
            DateTime operationDate,
            AccountDomainEntity fromAccount,
            AccountDomainEntity toAccount,
            decimal amount)
        {
            TransactionType = transactionType;
            OrderDesctiption = orderDescription;
            CreateDate = DateTime.Now;
            OperationDate = operationDate;
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Amount = amount;
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
    }

    public class TransactionOrderRepository :
        Repository<TransactionOrderDomainEntity, TransactionOrder>,
        ITransactionOrderRepository,
        IDomainObjectBuilderRepository<TransactionOrderDomainEntity, TransactionOrder>
    {
        private readonly IAccountRepository accountRepository;
        private readonly ITransactionTypeRepository transactionTypeRepository;

        public TransactionOrderRepository(
            ICoreContext coreContext, 
            AydoganFBankDbContext dbContext,
            IAccountRepository accountRepository,
            ITransactionTypeRepository transactionTypeRepository) 
            : base(coreContext, dbContext, null, null)
        {
            this.accountRepository = accountRepository;
            this.transactionTypeRepository = transactionTypeRepository;
        }

        #region Mapping overrides
        public override TransactionOrderDomainEntity MapToDomainObject(TransactionOrder dbEntity)
        {
            var domainEntity = coreContext.New<TransactionOrderDomainEntity>();
            MapToDomainObject(domainEntity, dbEntity);
            return domainEntity;
        }

        public override void MapToDomainObject(TransactionOrderDomainEntity domainEntity, TransactionOrder dbEntity)
        {
            domainEntity.Amount = dbEntity.Amount;
            domainEntity.CreateDate = dbEntity.CreateDate;
            domainEntity.FromAccount = accountRepository.GetById(dbEntity.FromAccountId);
            domainEntity.OperationDate = dbEntity.OperationDate;
            domainEntity.OrderDesctiption = dbEntity.OrderDescription;
            domainEntity.ToAccount = accountRepository.GetById(dbEntity.ToAccountId);
            domainEntity.TransactionOrderId = dbEntity.TransactionOrderId;
            domainEntity.TransactionType = transactionTypeRepository.GetById(dbEntity.TransactionTypeId);
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
    }

    public interface ITransactionOrderRepository : IRepository<TransactionOrderDomainEntity>
    {

    }
}
