using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;

namespace AydoganFBank.AccountManagement.Builder
{
    public class CreditCardExtreDomainEntityBuilder : IDomainEntityBuilder<CreditCardExtreDomainEntity, CreditCardExtre>
    {
        #region IoC
        private readonly ICoreContext coreContext;

        public CreditCardExtreDomainEntityBuilder(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }
        #endregion

        public CreditCardExtreDomainEntity MapToDomainObject(CreditCardExtre entity)
        {
            var domainEntity = coreContext.New<CreditCardExtreDomainEntity>();
            MapToDomainObject(domainEntity, entity);
            return domainEntity;
        }

        public void MapToDomainObject(CreditCardExtreDomainEntity domainEntity, CreditCardExtre entity)
        {
            if (domainEntity == null || entity == null)
                return;

            domainEntity.CreditCard = coreContext.Query<ICreditCardRepository>().GetById(entity.CreditCardId);
            domainEntity.CreditCardExtreId = entity.CreditCardExtreId;
            domainEntity.IsDischarged = entity.IsDischarged;
            domainEntity.IsMinDischarged = entity.IsMinDischarged;
            domainEntity.MinPayment = entity.MinPayment;
            domainEntity.Month = entity.Month;
            domainEntity.MonthName = entity.MonthName;
            domainEntity.TotalPayment = entity.TotalPayment;
            domainEntity.Year = entity.Year;
        }

        public IEnumerable<CreditCardExtreDomainEntity> MapToDomainObjectList(IEnumerable<CreditCardExtre> entities)
        {
            List<CreditCardExtreDomainEntity> domainEntities = new List<CreditCardExtreDomainEntity>();
            foreach (var entity in entities)
            {
                domainEntities.Add(MapToDomainObject(entity));
            }
            return domainEntities;
        }
    }

    public class CreditCardExtreMapper : IDbEntityMapper<CreditCardExtre, CreditCardExtreDomainEntity>
    {
        public void MapToDbEntity(CreditCardExtreDomainEntity domainEntity, CreditCardExtre dbEntity)
        {
            dbEntity.CreditCardId = domainEntity.CreditCard.CreditCardId;
            dbEntity.IsDischarged = domainEntity.IsDischarged;
            dbEntity.IsMinDischarged = domainEntity.IsMinDischarged;
            dbEntity.MinPayment = domainEntity.MinPayment;
            dbEntity.Month = domainEntity.Month;
            dbEntity.MonthName = domainEntity.MonthName;
            dbEntity.TotalPayment = domainEntity.TotalPayment;
            dbEntity.Year = domainEntity.Year;
        }
    }
}
