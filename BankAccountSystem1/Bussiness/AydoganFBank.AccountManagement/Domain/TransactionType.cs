using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Repository;
using AydoganFBank.Common;
using AydoganFBank.Common.Builders;
using AydoganFBank.Common.IoC;
using AydoganFBank.Database;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
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

    public class TransactionTypeRepository : 
        Repository<TransactionTypeDomainEntity, TransactionType>,
        ITransactionTypeRepository
    {
        public TransactionTypeRepository(
            ICoreContext coreContext,
            IDomainEntityBuilder<TransactionTypeDomainEntity, TransactionType> domainEntityBuilder, 
            IDbEntityMapper<TransactionType, TransactionTypeDomainEntity> dbEntityMapper) 
            : base(coreContext, domainEntityBuilder, dbEntityMapper)
        {
        }

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
