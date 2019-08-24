using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.IoC;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
{
    public class TransactionDetailDomainEntity : IDomainEntity, ITransactionHolder
    {
        #region Ioc
        private readonly ICoreContext coreContext;
        private readonly ITransactionDetailRepository transactionDetailRepository;

        public TransactionDetailDomainEntity(
            ICoreContext coreContext,
            ITransactionDetailRepository transactionDetailRepository)
        {
            this.coreContext = coreContext;
            this.transactionDetailRepository = transactionDetailRepository;
        }
        #endregion

        public int TransactionDetailId { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public AccountTransactionDomainEntity AccountTransaction { get; set; }
        public ITransactionOwner TransactionOwner { get; set; }
        public TransactionDirection TransactionDirection { get; set; }


        int IDomainEntity.Id => TransactionDetailId;

        ITransactionInfo ITransactionHolder.TransactionInfo => AccountTransaction;
        DateTime ITransactionHolder.CreateDate => CreateDate;


        public TransactionDetailDomainEntity With(
            string description,
            DateTime createDate,
            AccountTransactionDomainEntity accountTransaction,
            ITransactionOwner transactionOwner,
            TransactionDirection transactionDirection)
        {
            Description = description;
            CreateDate = createDate;
            AccountTransaction = accountTransaction;
            TransactionOwner = transactionOwner;
            TransactionDirection = transactionDirection;

            return this;
        }

        public void Insert()
        {
            transactionDetailRepository.InsertEntity(this);
        }

        public void Save()
        {
            transactionDetailRepository.UpdateEntity(this);
        }
    }

    public class TransactionDetailRepository :
        OrderedQueryRepository<TransactionDetailDomainEntity, TransactionDetail>,
        IDomainObjectBuilderRepository<TransactionDetailDomainEntity, TransactionDetail>,
        ITransactionDetailRepository
    {
        public TransactionDetailRepository(ICoreContext coreContext)
            : base(coreContext, null, null)
        {
        }

        public override TransactionDetailDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override TransactionDetail GetDbEntityById(int id)
        {
            return dbContext.TransactionDetail.FirstOrDefault(td => td.TransactionDetailId == id);
        }

        private ITransactionOwner GetTransactionOwner(int ownerType, int ownerId)
        {
            ITransactionOwner transactionOwner = null;

            if (ownerType == TransactionOwnerType.Account.ToInt())
                transactionOwner = coreContext.Query<IAccountRepository>().GetById(ownerId);
            else if (ownerType == TransactionOwnerType.CreditCard.ToInt())
                transactionOwner = coreContext.Query<ICreditCardRepository>().GetById(ownerId);
            else if (ownerType == TransactionOwnerType.TransactionOrder.ToInt())
                transactionOwner = coreContext.Query<ITransactionOrderRepository>().GetById(ownerId);

            return transactionOwner;
        }

        #region Mapping overrides
        public override void MapToDbEntity(TransactionDetailDomainEntity domainEntity, TransactionDetail dbEntity)
        {
            dbEntity.AccountTransactionId = domainEntity.AccountTransaction.TransactionId;
            dbEntity.CreateDate = domainEntity.CreateDate;
            dbEntity.Description = domainEntity.Description;
            dbEntity.OwnerId = domainEntity.TransactionOwner.OwnerId;
            dbEntity.OwnerType = domainEntity.TransactionOwner.OwnerType.ToInt();
            dbEntity.TransactionDirection = domainEntity.TransactionDirection.ToInt();
        }

        public override TransactionDetailDomainEntity MapToDomainObject(TransactionDetail dbEntity)
        {
            if (dbEntity == null)
                return null;

            var domainEntity = coreContext.New<TransactionDetailDomainEntity>();
            MapToDomainObject(domainEntity, dbEntity);
            return domainEntity;
        }

        public override void MapToDomainObject(TransactionDetailDomainEntity domainEntity, TransactionDetail dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.AccountTransaction = coreContext.Query<IAccountTransactionRepository>().GetById(dbEntity.AccountTransactionId);
            domainEntity.CreateDate = dbEntity.CreateDate;
            domainEntity.Description = dbEntity.Description;
            domainEntity.TransactionDetailId = dbEntity.TransactionDetailId;
            domainEntity.TransactionDirection = (TransactionDirection)Enum.Parse(typeof(TransactionDirection), dbEntity.TransactionDirection.ToString());
            domainEntity.TransactionOwner = GetTransactionOwner(dbEntity.OwnerType, dbEntity.OwnerId);
        }

        public override IEnumerable<TransactionDetailDomainEntity> MapToDomainObjectList(IEnumerable<TransactionDetail> dbEntities)
        {
            List<TransactionDetailDomainEntity> domainEntities = new List<TransactionDetailDomainEntity>();
            foreach (var dbEntity in dbEntities)
            {
                domainEntities.Add(MapToDomainObject(dbEntity));
            }
            return domainEntities;
        }
        #endregion

        public List<TransactionDetailDomainEntity> GetLastDateRangeListByTransactionOwner(
            ITransactionOwner transactionOwner, DateTime startDate, DateTime endDate)
        {
            return GetOrderedDescListBy(
                td =>
                    td.OwnerType == transactionOwner.OwnerType.ToInt() && td.OwnerId == transactionOwner.OwnerId && td.CreateDate >= startDate && td.CreateDate <= endDate,
                td =>
                    td.CreateDate)
                .ToList();                
        }

        public List<TransactionDetailDomainEntity> GetLastDateRangeAndTransactionDirectionListByTransactionOwner(
            ITransactionOwner transactionOwner, TransactionDirection transactionDirection, DateTime startDate, DateTime endDate)
        {
            return GetOrderedDescListBy(
                td =>
                    td.OwnerType == transactionOwner.OwnerType.ToInt() && td.OwnerId == transactionOwner.OwnerId && td.CreateDate >= startDate && td.CreateDate <= endDate &&
                    td.TransactionDirection == transactionDirection.ToInt(),
                td =>
                    td.CreateDate)
                .ToList();
        }
    }


    public interface ITransactionDetailRepository : IRepository<TransactionDetailDomainEntity>
    {
        List<TransactionDetailDomainEntity> GetLastDateRangeListByTransactionOwner(ITransactionOwner transactionOwner, DateTime startDate, DateTime endDate);
        List<TransactionDetailDomainEntity> GetLastDateRangeAndTransactionDirectionListByTransactionOwner(ITransactionOwner transactionOwner, TransactionDirection transactionDirection, DateTime startDate, DateTime endDate);
    }
}
