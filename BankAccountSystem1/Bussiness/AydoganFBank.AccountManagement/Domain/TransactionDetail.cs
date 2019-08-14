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
    public class TransactionDetailDomainEntity : 
        IDomainEntity
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
        ITransactionDetailRepository
    {
        public TransactionDetailRepository(
            ICoreContext coreContext, 
            IDomainEntityBuilder<TransactionDetailDomainEntity, TransactionDetail> domainEntityBuilder, 
            IDbEntityMapper<TransactionDetail, TransactionDetailDomainEntity> dbEntityMapper) 
            : base(coreContext, domainEntityBuilder, dbEntityMapper)
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

        public List<TransactionDetailDomainEntity> GetLastDateRangeListByTransactionOwner(
            ITransactionOwner transactionOwner, DateTime startDate, DateTime endDate)
        {
            return GetOrderedDescListBy(
                td =>
                    td.AccountId == transactionOwner.OwnerId && td.CreateDate >= startDate && td.CreateDate <= endDate,
                td =>
                    td.CreateDate)
                .ToList();                
        }

        public List<TransactionDetailDomainEntity> GetLastDateRangeAndTransactionDirectionListByTransactionOwner(
            ITransactionOwner transactionOwner, TransactionDirection transactionDirection, DateTime startDate, DateTime endDate)
        {
            return GetOrderedDescListBy(
                td =>
                    td.AccountId == transactionOwner.OwnerId && td.CreateDate >= startDate && td.CreateDate <= endDate &&
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
