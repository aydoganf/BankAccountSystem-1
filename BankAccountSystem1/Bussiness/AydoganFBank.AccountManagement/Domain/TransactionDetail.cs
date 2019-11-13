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
    public class TransactionDetailDomainEntity : IDomainEntity, ITransactionHolder, ITransactionDetailInfo
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
        public decimal Amount { get; set; }
        public AccountTransactionDomainEntity AccountTransaction { get; set; }
        public ITransactionDetailOwner TransactionDetailOwner { get; set; }
        public TransactionDirection TransactionDirection { get; set; }


        int IDomainEntity.Id => TransactionDetailId;

        ITransactionInfo ITransactionHolder.TransactionInfo => AccountTransaction;
        DateTime ITransactionHolder.CreateDate => CreateDate;

        int ITransactionDetailInfo.Id => TransactionDetailId;
        string ITransactionDetailInfo.Description => Description;
        ITransactionInfo ITransactionDetailInfo.TransactionInfo => AccountTransaction;
        TransactionDirection ITransactionDetailInfo.TransactionDirection => TransactionDirection;
        ITransactionDetailOwner ITransactionDetailInfo.TransactionDetailOwner => TransactionDetailOwner;
        decimal ITransactionDetailInfo.Amount => Amount;

        public TransactionDetailDomainEntity With(
            string description,
            DateTime createDate,
            decimal amount,
            AccountTransactionDomainEntity accountTransaction,
            ITransactionDetailOwner transactionDetailOwner,
            TransactionDirection transactionDirection)
        {
            Description = description;
            CreateDate = createDate;
            Amount = amount;
            AccountTransaction = accountTransaction;
            TransactionDetailOwner = transactionDetailOwner;
            TransactionDirection = transactionDirection;

            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            transactionDetailRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            transactionDetailRepository.UpdateEntity(this);
        }
    }

    public class TransactionDetailRepository : OrderedQueryRepository<TransactionDetailDomainEntity, TransactionDetail>, ITransactionDetailRepository
    {
        public TransactionDetailRepository(ICoreContext coreContext)
            : base(coreContext)
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

        private ITransactionDetailOwner GetTransactionDetailOwner(int ownerType, int ownerId)
        {
            ITransactionDetailOwner transactionDetailOwner = null;

            if (ownerType == TransactionDetailOwnerType.Account.ToInt())
                transactionDetailOwner = coreContext.Query<IAccountRepository>().GetById(ownerId);
            else if (ownerType == TransactionDetailOwnerType.CreditCard.ToInt())
                transactionDetailOwner = coreContext.Query<ICreditCardRepository>().GetById(ownerId);
            else if (ownerType == TransactionDetailOwnerType.CreditCardPayment.ToInt())
                transactionDetailOwner = coreContext.Query<ICreditCardPaymentRepository>().GetById(ownerId);
            else if (ownerType == TransactionDetailOwnerType.CreditCardExtreDischarge.ToInt())
                transactionDetailOwner = coreContext.Query<ICreditCardExtreDischargeRepository>().GetById(ownerId);
            return transactionDetailOwner;
        }

        #region Mapping overrides
        public override void MapToDbEntity(TransactionDetailDomainEntity domainEntity, TransactionDetail dbEntity)
        {
            dbEntity.AccountTransactionId = domainEntity.AccountTransaction.TransactionId;
            dbEntity.CreateDate = domainEntity.CreateDate;
            dbEntity.Description = domainEntity.Description;
            dbEntity.OwnerId = domainEntity.TransactionDetailOwner.OwnerId;
            dbEntity.OwnerType = domainEntity.TransactionDetailOwner.OwnerType.ToInt();
            dbEntity.TransactionDirection = domainEntity.TransactionDirection.ToInt();
            dbEntity.Amount = domainEntity.Amount;
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
            domainEntity.TransactionDetailOwner = GetTransactionDetailOwner(dbEntity.OwnerType, dbEntity.OwnerId);
            domainEntity.Amount = dbEntity.Amount;
        }
        #endregion

        public List<TransactionDetailDomainEntity> GetDateRangeListByTransactionDetailOwner(
            ITransactionDetailOwner transactionDetailOwner, DateTime startDate, DateTime endDate)
        {
            int ownerType = transactionDetailOwner.OwnerType.ToInt();

            return GetOrderedAscListBy(
                td =>
                    td.OwnerType == ownerType && td.OwnerId == transactionDetailOwner.OwnerId &&
                    td.CreateDate >= startDate && td.CreateDate <= endDate,
                td =>
                    td.CreateDate);
        }

        public List<TransactionDetailDomainEntity> GetLastDateRangeListByTransactionDetailOwner(
            ITransactionDetailOwner transactionOwner, DateTime startDate, DateTime endDate)
        {
            int ownerType = transactionOwner.OwnerType.ToInt();
            return GetOrderedDescListBy(
                td =>
                    td.OwnerType == ownerType && td.OwnerId == transactionOwner.OwnerId &&
                    td.CreateDate >= startDate && td.CreateDate <= endDate,
                td =>
                    td.CreateDate);        
        }

        public List<TransactionDetailDomainEntity> GetLastDateRangeAndTransactionDirectionListByTransactionDetailOwner(
            ITransactionDetailOwner transactionOwner, TransactionDirection transactionDirection, DateTime startDate, DateTime endDate)
        {
            int ownerType = transactionOwner.OwnerType.ToInt();
            return GetOrderedDescListBy(
                td =>
                    td.OwnerType == ownerType && td.OwnerId == transactionOwner.OwnerId &&
                    td.CreateDate >= startDate && td.CreateDate <= endDate && td.TransactionDirection == transactionDirection.ToInt(),
                td =>
                    td.CreateDate);
        }
    }


    public interface ITransactionDetailRepository : IRepository<TransactionDetailDomainEntity>
    {
        List<TransactionDetailDomainEntity> GetLastDateRangeListByTransactionDetailOwner(
            ITransactionDetailOwner transactionDetailOwner, DateTime startDate, DateTime endDate);
        List<TransactionDetailDomainEntity> GetLastDateRangeAndTransactionDirectionListByTransactionDetailOwner(
            ITransactionDetailOwner transactionDetailOwner, TransactionDirection transactionDirection, DateTime startDate, DateTime endDate);
        List<TransactionDetailDomainEntity> GetDateRangeListByTransactionDetailOwner(
            ITransactionDetailOwner transactionDetailOwner, DateTime startDate, DateTime endDate);
    }
}
