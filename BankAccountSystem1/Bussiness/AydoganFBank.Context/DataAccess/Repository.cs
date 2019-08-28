using AydoganFBank.Context.Builders;
using AydoganFBank.Context.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace AydoganFBank.Context.DataAccess
{
    public abstract class Repository<TDomainEntity, TDbEntity>
        where TDomainEntity : IDomainEntity
        where TDbEntity : class
    {
        protected readonly ICoreContext coreContext;
        protected readonly AydoganFBankDbContext dbContext;
        protected readonly IDomainEntityBuilder<TDomainEntity, TDbEntity> domainEntityBuilder;
        protected readonly IDbEntityMapper<TDbEntity, TDomainEntity> dbEntityMapper;

        public Repository(
            ICoreContext coreContext,
            IDomainEntityBuilder<TDomainEntity, TDbEntity> domainEntityBuilder,
            IDbEntityMapper<TDbEntity, TDomainEntity> dbEntityMapper)
        {
            this.coreContext = coreContext;
            this.domainEntityBuilder = domainEntityBuilder;
            this.dbEntityMapper = dbEntityMapper;

            dbContext = this.coreContext.DBContext;
        }

        public virtual TDomainEntity MapToDomainObject(TDbEntity dbEntity)
        {
            if (dbEntity == null)
                return default(TDomainEntity);

            var domainEnttiy = coreContext.New<TDomainEntity>();
            MapToDomainObject(domainEnttiy, dbEntity);
            return domainEnttiy;
        }
        public abstract void MapToDomainObject(TDomainEntity domainEntity, TDbEntity dbEntity);

        public virtual IEnumerable<TDomainEntity> MapToDomainObjectList(IEnumerable<TDbEntity> dbEntities)
        {
            List<TDomainEntity> domainEntities = new List<TDomainEntity>();
            foreach (var dbEntity in dbEntities)
            {
                domainEntities.Add(MapToDomainObject(dbEntity));
            }
            return domainEntities;
        }

        public abstract void MapToDbEntity(TDomainEntity domainEntity, TDbEntity dbEntity);

        private void UpdateEntity(TDbEntity dbEntity)
        {
            var entry = dbContext.Entry(dbEntity);
            entry.State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public void UpdateEntity(TDomainEntity domainEntity)
        {
            TDbEntity dbEntity = GetDbEntityById(domainEntity.Id);
            MapToDbEntity(domainEntity, dbEntity);
            UpdateEntity(dbEntity);
        }

        protected abstract TDbEntity GetDbEntityById(int id);

        protected virtual void InsertEntity(TDbEntity dbEntity, bool forceToInsertDb = true)
        {
            var entry = dbContext.Entry(dbEntity);
            entry.State = EntityState.Added;
            if (forceToInsertDb)
                dbContext.SaveChanges();
        }

        public void InsertEntity(TDomainEntity domainEntity, TDbEntity dbEntity, bool forceToInsertDb = true)
        {
            MapToDbEntity(domainEntity, dbEntity);
            InsertEntity(dbEntity, forceToInsertDb);
            if (forceToInsertDb)
                MapToDomainObject(domainEntity, dbEntity);
        }

        public void InsertEntity(TDomainEntity domainEntity, bool forceToInsertDb = true)
        {
            TDbEntity dbEntity = coreContext.New<TDbEntity>();
            InsertEntity(domainEntity, dbEntity, forceToInsertDb);
        }

        public abstract TDomainEntity GetById(int id);

        public IEnumerable<TDomainEntity> GetAll()
        {
            return MapToDomainObjectList(dbContext.Set<TDbEntity>().ToList());
        }

        protected TDomainEntity GetFirstBy(Expression<Func<TDbEntity, bool>> predicate)
        {
            return MapToDomainObject(dbContext.Set<TDbEntity>().FirstOrDefault(predicate));
        }

        protected IEnumerable<TDomainEntity> GetListBy(Expression<Func<TDbEntity, bool>> predicate)
        {
            return MapToDomainObjectList(dbContext.Set<TDbEntity>().Where(predicate));
        }
    }
}
