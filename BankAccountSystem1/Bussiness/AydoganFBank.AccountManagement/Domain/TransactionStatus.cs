using AydoganFBank.AccountManagement.Repository;
using AydoganFBank.Common;
using AydoganFBank.Common.Builders;
using AydoganFBank.Common.IoC;
using AydoganFBank.Database;
using System.Linq;
using TransactionStatus = AydoganFBank.Database.TransactionStatus;

namespace AydoganFBank.AccountManagement.Domain
{
    public class TransactionStatusDomainEntity : IDomainEntity
    {
        #region IoC
        private readonly ITransactionStatusRepository transactionStatusRepository;

        public TransactionStatusDomainEntity(ITransactionStatusRepository transactionStatusRepository)
        {
            this.transactionStatusRepository = transactionStatusRepository;
        }
        #endregion

        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusKey { get; set; }

        int IDomainEntity.Id => StatusId;

        public TransactionStatusDomainEntity With(
            string statusName,
            string statusKey)
        {
            StatusName = statusName;
            StatusKey = statusKey;
            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            transactionStatusRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            transactionStatusRepository.UpdateEntity(this);
        }
    }

    public class TransactionStatusRepository :
        Repository<TransactionStatusDomainEntity, TransactionStatus>,
        ITransactionStatusRepository
    {
        public TransactionStatusRepository(
            ICoreContext coreContext, 
            AydoganFBankDbContext dbContext, 
            IDomainEntityBuilder<TransactionStatusDomainEntity, TransactionStatus> domainEntityBuilder, 
            IDbEntityMapper<TransactionStatus, TransactionStatusDomainEntity> dbEntityMapper) 
            : base(coreContext, dbContext, domainEntityBuilder, dbEntityMapper)
        {
        }

        public override TransactionStatusDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override TransactionStatus GetDbEntityById(int id)
        {
            return dbContext.TransactionStatus.FirstOrDefault(ts => ts.TransactionStatusId == id);
        }

        public TransactionStatusDomainEntity GetByKey(string statusKey)
        {
            return GetFirstBy(ts => ts.StatusKey == statusKey);
        }
    }

    public interface ITransactionStatusRepository : IRepository<TransactionStatusDomainEntity>
    {
        TransactionStatusDomainEntity GetByKey(string statusKey);
    }
}
