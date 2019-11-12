using AydoganFBank.TransactionManagement.Api;
using AydoganFBank.Context;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.IoC;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Database;
using System.Linq;

namespace AydoganFBank.TransactionManagement.Domain
{
    public class TransactionTypeDomainEntity : IDomainEntity, ITransactionTypeInfo
    {
        #region IoC
        private readonly ITransactionTypeRepository transactionTypeRepository;

        public TransactionTypeDomainEntity(ITransactionTypeRepository transactionTypeRepository)
        {
            this.transactionTypeRepository = transactionTypeRepository;
        }
        #endregion

        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string TypeKey { get; set; }

        int IDomainEntity.Id => TypeId;

        public TransactionTypeDomainEntity With(
            string typeName,
            string typeKey)
        {
            TypeName = typeName;
            TypeKey = typeKey;
            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            transactionTypeRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            transactionTypeRepository.UpdateEntity(this);
        }
    }

    public class TransactionTypeRepository : Repository<TransactionTypeDomainEntity, TransactionType>, ITransactionTypeRepository
    {
        public TransactionTypeRepository(
            ICoreContext coreContext) 
            : base(coreContext)
        {
        }

        #region Mapping overrides
        public override void MapToDbEntity(TransactionTypeDomainEntity domainEntity, TransactionType dbEntity)
        {
            dbEntity.TypeKey = domainEntity.TypeKey;
            dbEntity.TypeName = domainEntity.TypeName;
        }

        public override void MapToDomainObject(TransactionTypeDomainEntity domainEntity, TransactionType dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.TypeId = dbEntity.TransactionTypeId;
            domainEntity.TypeKey = dbEntity.TypeKey;
            domainEntity.TypeName = dbEntity.TypeName;
        }
        #endregion

        public override TransactionTypeDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override TransactionType GetDbEntityById(int id)
        {
            return dbContext.TransactionType.FirstOrDefault(tt => tt.TransactionTypeId == id);
        }

        public TransactionTypeDomainEntity GetByKey(string typeKey)
        {
            return GetFirstBy(tt => tt.TypeKey == typeKey);
        }
    }

    public interface ITransactionTypeRepository : IRepository<TransactionTypeDomainEntity>
    {
        TransactionTypeDomainEntity GetByKey(string typeKey);
    }
}
