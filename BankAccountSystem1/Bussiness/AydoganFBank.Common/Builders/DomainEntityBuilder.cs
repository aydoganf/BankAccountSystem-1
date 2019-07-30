using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Common.Builders
{
    public interface IDomainEntityBuilder<TDomainEntity, TDbEntity>
        where TDomainEntity : IDomainEntity
        where TDbEntity : class
    {
        TDomainEntity MapToDomainObject(TDbEntity entity);

        IEnumerable<TDomainEntity> MapToDomainObjectList(IEnumerable<TDbEntity> entities);
    }
}
