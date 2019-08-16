using AydoganFBank.Common.Builders;

namespace AydoganFBank.Common.Repository
{
    public interface IDomainObjectBuilderRepository<TDomainEntity, TDbEntity> : IDomainEntityBuilder<TDomainEntity, TDbEntity>
        where TDomainEntity : IDomainEntity
        where TDbEntity : class
    {
    }
}
