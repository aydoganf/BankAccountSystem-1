using AydoganFBank.Common;
using AydoganFBank.Common.Builders;
using AydoganFBank.Common.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AydoganFBank.AccountManagement.Repository
{
    public abstract class OrderedQueryRepository<TDomainEntity, TDbEntity> : Repository<TDomainEntity, TDbEntity>
        where TDomainEntity : IDomainEntity
        where TDbEntity : class
    {
        public OrderedQueryRepository(
            ICoreContext coreContext, 
            AydoganFBankDbContext dbContext, 
            IDomainEntityBuilder<TDomainEntity, TDbEntity> domainEntityBuilder, 
            IDbEntityMapper<TDbEntity, TDomainEntity> dbEntityMapper) 
            : base(coreContext, dbContext, domainEntityBuilder, dbEntityMapper)
        {
        }

        protected IEnumerable<TDomainEntity> GetOrderedAscListBy<TOrder>(
            Expression<Func<TDbEntity, bool>> predicate,
            Expression<Func<TDbEntity, TOrder>> ascendingOrder)
        {
            return MapToDomainObjectList(
                    dbContext.Set<TDbEntity>().Where(predicate)
                    .OrderBy(ascendingOrder));
        }

        protected IEnumerable<TDomainEntity> GetOrderedAscListBy<TOrder1, TOrder2>(
            Expression<Func<TDbEntity, bool>> predicate,
            Expression<Func<TDbEntity, TOrder1>> ascendingOrder,
            Expression<Func<TDbEntity, TOrder2>> ascendingOrderThen)
        {
            return MapToDomainObjectList(
                    dbContext.Set<TDbEntity>().Where(predicate)
                    .OrderBy(ascendingOrder)
                    .ThenBy(ascendingOrderThen));
        }

        protected IEnumerable<TDomainEntity> GetOrderedAscListBy<TOrder1, TOrder2, TOrder3>(
            Expression<Func<TDbEntity, bool>> predicate,
            Expression<Func<TDbEntity, TOrder1>> ascendingOrder,
            Expression<Func<TDbEntity, TOrder2>> ascendingOrderThen1,
            Expression<Func<TDbEntity, TOrder3>> ascendingOrderThen2)
        {
            return MapToDomainObjectList(
                    dbContext.Set<TDbEntity>().Where(predicate)
                    .OrderBy(ascendingOrder)
                    .ThenBy(ascendingOrderThen1)
                    .ThenBy(ascendingOrderThen2));
        }

        protected TDomainEntity GetLastBy<TOrder>(
            Expression<Func<TDbEntity, TOrder>> descendingOrder)
        {
            return MapToDomainObject(
                dbContext.Set<TDbEntity>().OrderByDescending(descendingOrder).FirstOrDefault());
        }

        protected TDomainEntity GetLastBy<TOrder>(
            Expression<Func<TDbEntity, bool>> predicate,
            Expression<Func<TDbEntity, TOrder>> descendingOrder)
        {
            return MapToDomainObject(
                dbContext.Set<TDbEntity>().Where(predicate).OrderByDescending(descendingOrder).FirstOrDefault());
        }

        protected IEnumerable<TDomainEntity> GetOrderedDescListBy<TOrder>(
            Expression<Func<TDbEntity, bool>> predicate,
            Expression<Func<TDbEntity, TOrder>> descendingOrder)
        {
            return MapToDomainObjectList(
                    dbContext.Set<TDbEntity>().Where(predicate).OrderByDescending(descendingOrder));
        }

        protected IEnumerable<TDomainEntity> GetOrderedDescListBy<TOrder1, TOrder2>(
            Expression<Func<TDbEntity, bool>> predicate,
            Expression<Func<TDbEntity, TOrder1>> descendingOrder1,
            Expression<Func<TDbEntity, TOrder1>> descendingOrder2)
        {
            return MapToDomainObjectList(
                    dbContext.Set<TDbEntity>().Where(predicate)
                    .OrderByDescending(descendingOrder1)
                    .ThenByDescending(descendingOrder2));
        }

        protected IEnumerable<TDomainEntity> GetOrderedDescListBy<TOrder1, TOrder2, TOrder3>(
            Expression<Func<TDbEntity, bool>> predicate,
            Expression<Func<TDbEntity, TOrder1>> descendingOrder1,
            Expression<Func<TDbEntity, TOrder1>> descendingOrder2,
            Expression<Func<TDbEntity, TOrder1>> descendingOrder3)
        {
            return MapToDomainObjectList(
                    dbContext.Set<TDbEntity>().Where(predicate)
                    .OrderByDescending(descendingOrder1)
                    .ThenByDescending(descendingOrder2)
                    .ThenByDescending(descendingOrder3));
        }
    }
}
