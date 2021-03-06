﻿using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Builder
{
    public class TransactionTypeDomainEntityBuilder :
        IDomainEntityBuilder<TransactionTypeDomainEntity, TransactionType>
    {
        private readonly ICoreContext coreContext;

        public TransactionTypeDomainEntityBuilder(
            ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }

        public TransactionTypeDomainEntity MapToDomainObject(TransactionType entity)
        {
            if (entity == null)
                return null;

            TransactionTypeDomainEntity domainEntity = coreContext.New<TransactionTypeDomainEntity>();
            MapToDomainObject(domainEntity, entity);
            return domainEntity;
        }

        public void MapToDomainObject(TransactionTypeDomainEntity domainEntity, TransactionType entity)
        {
            if (domainEntity == null || entity == null)
                return;

            domainEntity.TypeId = entity.TransactionTypeId;
            domainEntity.TypeKey = entity.TypeKey;
            domainEntity.TypeName = entity.TypeName;
        }

        public IEnumerable<TransactionTypeDomainEntity> MapToDomainObjectList(IEnumerable<TransactionType> entities)
        {
            List<TransactionTypeDomainEntity> domainEntities = new List<TransactionTypeDomainEntity>();
            foreach (var entity in entities)
            {
                domainEntities.Add(MapToDomainObject(entity));
            }
            return domainEntities;
        }
    }

    public class TransactionTypeMapper : IDbEntityMapper<TransactionType, TransactionTypeDomainEntity>
    {
        public void MapToDbEntity(TransactionTypeDomainEntity domainEntity, TransactionType dbEntity)
        {
            dbEntity.TypeKey = domainEntity.TypeKey;
            dbEntity.TypeName = domainEntity.TypeName;
        }
    }
}
