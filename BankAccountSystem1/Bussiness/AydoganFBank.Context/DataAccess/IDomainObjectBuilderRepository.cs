using AydoganFBank.Context.Builders;

namespace AydoganFBank.Context.DataAccess
{
    public interface IDomainObjectBuilderRepository<TDomainEntity, TDbEntity> : IDomainEntityBuilder<TDomainEntity, TDbEntity>
        where TDomainEntity : IDomainEntity
        where TDbEntity : class
    {
    }
}
