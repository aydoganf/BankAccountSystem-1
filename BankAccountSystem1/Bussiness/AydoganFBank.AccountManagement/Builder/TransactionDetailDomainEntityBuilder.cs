using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.Common.Builders;
using AydoganFBank.Common.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Builder
{
    public class TransactionDetailDomainEntityBuilder :
        IDomainEntityBuilder<TransactionDetailDomainEntity, TransactionDetail>
    {
        #region IoC
        private readonly ICoreContext coreContext;

        public TransactionDetailDomainEntityBuilder(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }
        #endregion

        public TransactionDetailDomainEntity MapToDomainObject(TransactionDetail entity)
        {
            if (entity == null)
                return null;

            TransactionDetailDomainEntity domainEntity = coreContext.New<TransactionDetailDomainEntity>();
            MapToDomainObject(domainEntity, entity);
            return domainEntity;
        }

        public void MapToDomainObject(TransactionDetailDomainEntity domainEntity, TransactionDetail entity)
        {
            if (entity == null)
                return;

            domainEntity.TransactionOwner = coreContext.Query<IAccountRepository>().GetById(entity.AccountId);
            domainEntity.AccountTransaction = coreContext.Query<IAccountTransactionRepository>().GetById(entity.AccountTransactionId);
            domainEntity.CreateDate = entity.CreateDate;
            domainEntity.Description = entity.Description;
            domainEntity.TransactionDetailId = entity.TransactionDetailId;
            domainEntity.TransactionDirection = (TransactionDirection)Enum.Parse(typeof(TransactionDirection), entity.TransactionDirection.ToString());
        }

        public IEnumerable<TransactionDetailDomainEntity> MapToDomainObjectList(IEnumerable<TransactionDetail> entities)
        {
            List<TransactionDetailDomainEntity> domainEntities = new List<TransactionDetailDomainEntity>();
            foreach (var entity in entities)
            {
                domainEntities.Add(MapToDomainObject(entity));
            }
            return domainEntities;
        }
    }

    public class TransactionDetailMapper : IDbEntityMapper<TransactionDetail, TransactionDetailDomainEntity>
    {
        public void MapToDbEntity(TransactionDetailDomainEntity domainEntity, TransactionDetail dbEntity)
        {
            dbEntity.AccountId = domainEntity.TransactionOwner.OwnerId;
            dbEntity.AccountTransactionId = domainEntity.AccountTransaction.TransactionId;
            dbEntity.CreateDate = domainEntity.CreateDate;
            dbEntity.Description = domainEntity.Description;
            dbEntity.TransactionDirection = domainEntity.TransactionDirection.ToInt();
        }
    }
}
