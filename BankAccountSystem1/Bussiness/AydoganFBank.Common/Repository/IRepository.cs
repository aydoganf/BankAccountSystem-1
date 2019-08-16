using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Common.Repository
{
    public interface IRepository<TDomainEntity> : IQueryRepository 
        where TDomainEntity : IDomainEntity
    {
        TDomainEntity GetById(int id);
        void InsertEntity(TDomainEntity domainEntity, bool forceToInsertDb = true);
        void UpdateEntity(TDomainEntity domainEntity);
    }

    public interface IQueryRepository
    {

    }
}
