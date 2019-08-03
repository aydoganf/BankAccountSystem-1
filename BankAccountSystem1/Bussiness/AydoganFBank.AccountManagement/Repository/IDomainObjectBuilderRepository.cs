using AydoganFBank.Common;
using AydoganFBank.Common.Builders;

namespace AydoganFBank.AccountManagement.Repository
{
    public interface IDomainObjectBuilderRepository<TDomainEntity, TDbEntity> : IDomainEntityBuilder<TDomainEntity, TDbEntity>
        where TDomainEntity : IDomainEntity
        where TDbEntity : class
    {
    }
}
