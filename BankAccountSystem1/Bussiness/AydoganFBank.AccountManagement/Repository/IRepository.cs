using AydoganFBank.Common;

namespace AydoganFBank.AccountManagement.Repository
{
    public interface IRepository<TDomainEntity> 
        where TDomainEntity : IDomainEntity
    {

        TDomainEntity GetById(int id);
        void InsertEntity(TDomainEntity domainEntity, bool forceToInsertDb = true);
        void UpdateEntity(TDomainEntity domainEntity);
    }
}
