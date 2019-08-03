﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Common.Builders
{
    public interface IDbEntityMapper<TDbEntity, TDomainEntity>
        where TDbEntity : class
        where TDomainEntity : IDomainEntity
    {
        void MapToDbEntity(TDomainEntity domainEntity, TDbEntity dbEntity);
    }
}
