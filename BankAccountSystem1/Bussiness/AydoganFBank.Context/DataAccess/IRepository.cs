using AydoganFBank.Context.IoC.Lifecycle;

namespace AydoganFBank.Context.DataAccess
{
    public interface IRepository<TDomainEntity> : IQueryRepository 
        where TDomainEntity : IDomainEntity
    {
        TDomainEntity GetById(int id);
        void InsertEntity(TDomainEntity domainEntity, bool forceToInsertDb = true);
        void UpdateEntity(TDomainEntity domainEntity);
    }

    public interface IQueryRepository : ITransientObject
    {

    }
}
