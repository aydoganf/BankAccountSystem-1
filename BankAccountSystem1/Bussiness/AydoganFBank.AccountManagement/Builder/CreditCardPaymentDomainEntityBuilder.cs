using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Builder
{
    public class CreditCardPaymentDomainEntityBuilder : IDomainEntityBuilder<CreditCardPaymentDomainEntity, CreditCardPayment>
    {
        #region IoC
        private readonly ICoreContext coreContext;

        public CreditCardPaymentDomainEntityBuilder(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }
        #endregion

        public CreditCardPaymentDomainEntity MapToDomainObject(CreditCardPayment entity)
        {
            if (entity == null)
                return null;

            var domaintEntity = coreContext.New<CreditCardPaymentDomainEntity>();
            MapToDomainObject(domaintEntity, entity);
            return domaintEntity;
        }

        public void MapToDomainObject(CreditCardPaymentDomainEntity domainEntity, CreditCardPayment entity)
        {
            if (domainEntity == null || entity == null)
                return;

            domainEntity.AccountTransaction = coreContext.Query<IAccountTransactionRepository>().GetById(entity.AccountTransactionId);
            domainEntity.Amount = entity.Amount;
            domainEntity.CreateDate = entity.CreateDate;
            domainEntity.CreditCardPaymentId = entity.CreditCardPaymentId;
            domainEntity.Description = entity.Description;
            domainEntity.InstalmentDate = entity.InstalmentDate;
            domainEntity.InstalmentIndex = entity.InstalmentIndex;
        }

        public IEnumerable<CreditCardPaymentDomainEntity> MapToDomainObjectList(IEnumerable<CreditCardPayment> entities)
        {
            List<CreditCardPaymentDomainEntity> domainEntities = new List<CreditCardPaymentDomainEntity>();
            foreach (var entity in entities)
            {
                domainEntities.Add(MapToDomainObject(entity));
            }
            return domainEntities;
        }
    }

    public class CreditCardPaymentMapper : IDbEntityMapper<CreditCardPayment, CreditCardPaymentDomainEntity>
    {
        public void MapToDbEntity(CreditCardPaymentDomainEntity domainEntity, CreditCardPayment dbEntity)
        {
            dbEntity.AccountTransactionId = domainEntity.AccountTransaction.TransactionId;
            dbEntity.Amount = domainEntity.Amount;
            dbEntity.CreateDate = domainEntity.CreateDate;
            dbEntity.Description = domainEntity.Description;
            dbEntity.InstalmentDate = domainEntity.InstalmentDate;
            dbEntity.InstalmentIndex = domainEntity.InstalmentIndex;
        }
    }
}
