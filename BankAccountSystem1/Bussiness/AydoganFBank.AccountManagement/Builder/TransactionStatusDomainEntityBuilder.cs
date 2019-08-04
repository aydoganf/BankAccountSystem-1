using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.Common.Builders;
using AydoganFBank.Common.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Builder
{
    public class TransactionStatusDomainEntityBuilder :
        IDomainEntityBuilder<TransactionStatusDomainEntity, TransactionStatus>
    {
        private readonly ICoreContext coreContext;

        public TransactionStatusDomainEntityBuilder(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }

        public TransactionStatusDomainEntity MapToDomainObject(TransactionStatus entity)
        {
            var domainEntity = coreContext.New<TransactionStatusDomainEntity>();
            MapToDomainObject(domainEntity, entity);
            return domainEntity;
        }

        public void MapToDomainObject(TransactionStatusDomainEntity domainEntity, TransactionStatus entity)
        {
            domainEntity.StatusId = entity.TransactionStatusId;
            domainEntity.StatusKey = entity.StatusKey;
            domainEntity.StatusName = entity.StatusName;
        }

        public IEnumerable<TransactionStatusDomainEntity> MapToDomainObjectList(IEnumerable<TransactionStatus> entities)
        {
            List<TransactionStatusDomainEntity> domainEntities = new List<TransactionStatusDomainEntity>();
            foreach (var entity in entities)
            {
                domainEntities.Add(MapToDomainObject(entity));
            }
            return domainEntities;
        }
    }
}
