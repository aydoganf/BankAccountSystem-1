using AydoganFBank.TransactionManagement.Api;
using AydoganFBank.Context;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.IoC;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AydoganFBank.TransactionManagement.Domain
{
    public class TransactionDetailDomainEntity : IDomainEntity
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
        public TransactionDirection TransactionDirection { get; set; }


        int IDomainEntity.Id => TransactionDetailId;

        public TransactionDetailDomainEntity With(
            string description,
            DateTime createDate,
            AccountTransactionDomainEntity accountTransaction,
            TransactionDirection transactionDirection)
        {
            Description = description;
            CreateDate = createDate;
            AccountTransaction = accountTransaction;
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
    }


    public interface ITransactionDetailRepository : IRepository<TransactionDetailDomainEntity>
    {
    }
}
