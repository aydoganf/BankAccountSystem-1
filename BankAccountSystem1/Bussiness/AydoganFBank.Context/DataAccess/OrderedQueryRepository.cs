using AydoganFBank.Context.Builders;
using AydoganFBank.Context.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AydoganFBank.Context.DataAccess
{
    public abstract class OrderedQueryRepository<TDomainEntity, TDbEntity> : Repository<TDomainEntity, TDbEntity>
        where TDomainEntity : IDomainEntity
        where TDbEntity : class
    {
        public OrderedQueryRepository(
            ICoreContext coreContext)
            : base(coreContext)
        {
        }

        protected List<TDomainEntity> GetOrderedAscListBy<TOrder>(
            Expression<Func<TDbEntity, bool>> whereCondition,
            Expression<Func<TDbEntity, TOrder>> ascendingOrder)
        {
            return MapToDomainObjectList(
                    dbContext.Set<TDbEntity>().Where(whereCondition)
                    .OrderBy(ascendingOrder));
        }

        protected List<TDomainEntity> GetOrderedAscListBy<TOrder1, TOrder2>(
            Expression<Func<TDbEntity, bool>> whereCondition,
            Expression<Func<TDbEntity, TOrder1>> ascendingOrder,
            Expression<Func<TDbEntity, TOrder2>> ascendingOrderThen)
        {
            return MapToDomainObjectList(
                    dbContext.Set<TDbEntity>().Where(whereCondition)
                    .OrderBy(ascendingOrder)
                    .ThenBy(ascendingOrderThen));
        }

        protected List<TDomainEntity> GetOrderedAscListBy<TOrder1, TOrder2, TOrder3>(
            Expression<Func<TDbEntity, bool>> whereCondition,
            Expression<Func<TDbEntity, TOrder1>> ascendingOrder,
            Expression<Func<TDbEntity, TOrder2>> ascendingOrderThen1,
            Expression<Func<TDbEntity, TOrder3>> ascendingOrderThen2)
        {
            return MapToDomainObjectList(
                    dbContext.Set<TDbEntity>().Where(whereCondition)
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
            Expression<Func<TDbEntity, bool>> whereCondition,
            Expression<Func<TDbEntity, TOrder>> descendingOrder)
        {
            return MapToDomainObject(
                dbContext.Set<TDbEntity>().Where(whereCondition).OrderByDescending(descendingOrder).FirstOrDefault());
        }

        protected List<TDomainEntity> GetLastItemCountListBy<TOrder>(
            Expression<Func<TDbEntity, TOrder>> descedingOrder,
            int itemCount)
        {
            return MapToDomainObjectList(
                dbContext.Set<TDbEntity>().OrderByDescending(descedingOrder).Take(itemCount));
        }

        protected List<TDomainEntity> GetLastItemCountListBy<TOrder>(
            Expression<Func<TDbEntity, bool>> whereCondition,
            Expression<Func<TDbEntity, TOrder>> descedingOrder,
            int itemCount)
        {
            return MapToDomainObjectList(
                dbContext.Set<TDbEntity>().Where(whereCondition).OrderByDescending(descedingOrder).Take(itemCount));
        }

        protected List<TDomainEntity> GetOrderedDescListBy<TOrder>(
            Expression<Func<TDbEntity, bool>> whereCondition,
            Expression<Func<TDbEntity, TOrder>> descendingOrder)
        {
            return MapToDomainObjectList(
                    dbContext.Set<TDbEntity>().Where(whereCondition).OrderByDescending(descendingOrder));
        }

        protected List<TDomainEntity> GetOrderedDescListBy<TOrder1, TOrder2>(
            Expression<Func<TDbEntity, bool>> whereCondition,
            Expression<Func<TDbEntity, TOrder1>> descendingOrder1,
            Expression<Func<TDbEntity, TOrder2>> descendingOrder2)
        {
            return MapToDomainObjectList(
                    dbContext.Set<TDbEntity>().Where(whereCondition)
                    .OrderByDescending(descendingOrder1)
                    .ThenByDescending(descendingOrder2));
        }

        protected List<TDomainEntity> GetOrderedDescListBy<TOrder1, TOrder2, TOrder3>(
            Expression<Func<TDbEntity, bool>> whereCondition,
            Expression<Func<TDbEntity, TOrder1>> descendingOrder1,
            Expression<Func<TDbEntity, TOrder2>> descendingOrder2,
            Expression<Func<TDbEntity, TOrder3>> descendingOrder3)
        {
            return MapToDomainObjectList(
                    dbContext.Set<TDbEntity>().Where(whereCondition)
                    .OrderByDescending(descendingOrder1)
                    .ThenByDescending(descendingOrder2)
                    .ThenByDescending(descendingOrder3));
        }
    }
}
