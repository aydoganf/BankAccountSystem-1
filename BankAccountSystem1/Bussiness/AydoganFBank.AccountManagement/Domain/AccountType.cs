using AydoganFBank.Common;
using AydoganFBank.Common.Builders;
using AydoganFBank.Common.IoC;
using AydoganFBank.Common.Repository;
using AydoganFBank.Database;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
{
    public class AccountTypeDomainEntity : IDomainEntity
    {
        #region IoC
        private readonly IAccountTypeRepository accountTypeRepository;

        public AccountTypeDomainEntity(IAccountTypeRepository accountTypeRepository)
        {
            this.accountTypeRepository = accountTypeRepository;
        }
        #endregion

        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountTypeKey { get; set; }
        public string AssetsUnit { get; set; }

        int IDomainEntity.Id => AccountTypeId;

        public AccountTypeDomainEntity With(
            string typeName, 
            string typeKey)
        {
            AccountTypeName = typeName;
            AccountTypeKey = typeKey;
            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            accountTypeRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            accountTypeRepository.UpdateEntity(this);
        }
    }

    public class AccountTypeRepository :
        Repository<AccountTypeDomainEntity, AccountType>,
        IAccountTypeRepository
    {
        public AccountTypeRepository(
            ICoreContext coreContext,  
            IDomainEntityBuilder<AccountTypeDomainEntity, AccountType> domainEntityBuilder, 
            IDbEntityMapper<AccountType, AccountTypeDomainEntity> dbEntityMapper) 
            : base(coreContext, domainEntityBuilder, dbEntityMapper)
        {
        }

        public override AccountTypeDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override AccountType GetDbEntityById(int id)
        {
            return dbContext.AccountType.FirstOrDefault(at => at.AccountTypeId == id);
        }

        public AccountTypeDomainEntity GetByKey(string typeKey)
        {
            return GetFirstBy(
                at =>
                    at.TypeKey == typeKey);
        }
    }

    public interface IAccountTypeRepository : IRepository<AccountTypeDomainEntity>
    {
        AccountTypeDomainEntity GetByKey(string typeKey);
    }
}
