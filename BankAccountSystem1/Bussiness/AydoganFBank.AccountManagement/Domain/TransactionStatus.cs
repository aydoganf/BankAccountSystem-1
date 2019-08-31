using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.IoC;
using AydoganFBank.Context.DataAccess;
using System.Linq;
using TransactionStatus = AydoganFBank.Database.TransactionStatus;
using AydoganFBank.Database;

namespace AydoganFBank.AccountManagement.Domain
{
    public class TransactionStatusDomainEntity : IDomainEntity, ITransactionStatusInfo
    {
        #region IoC
        private readonly ITransactionStatusRepository transactionStatusRepository;

        public TransactionStatusDomainEntity(ITransactionStatusRepository transactionStatusRepository)
        {
            this.transactionStatusRepository = transactionStatusRepository;
        }
        #endregion

        public string StatusName { get; set; }
        public int StatusId { get; set; }
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

    public class TransactionStatusRepository : Repository<TransactionStatusDomainEntity, TransactionStatus>, ITransactionStatusRepository
    {
        public TransactionStatusRepository(
            ICoreContext coreContext) 
            : base(coreContext)
        {
        }

        #region Mapping overrides
        public override void MapToDbEntity(TransactionStatusDomainEntity domainEntity, TransactionStatus dbEntity)
        {
            dbEntity.StatusKey = domainEntity.StatusKey;
            dbEntity.StatusName = domainEntity.StatusName;
        }

        public override void MapToDomainObject(TransactionStatusDomainEntity domainEntity, TransactionStatus dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.StatusId = dbEntity.TransactionStatusId;
            domainEntity.StatusKey = dbEntity.StatusKey;
            domainEntity.StatusName = dbEntity.StatusName;
        }
        #endregion

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
