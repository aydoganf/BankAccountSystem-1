﻿using AydoganFBank.Context.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Context.Builders
{
    public interface IDomainEntityBuilder<TDomainEntity, TDbEntity>
        where TDomainEntity : IDomainEntity
        where TDbEntity : class
    {
        TDomainEntity MapToDomainObject(TDbEntity entity);
        void MapToDomainObject(TDomainEntity domainEntity, TDbEntity entity);

        IEnumerable<TDomainEntity> MapToDomainObjectList(IEnumerable<TDbEntity> entities);
    }
}
