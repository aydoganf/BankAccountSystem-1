using AydoganFBank.Context.Builders;
using AydoganFBank.Context.Exception;
using AydoganFBank.Context.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
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

        public Repository(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
            dbContext = this.coreContext.DBContext;
        }

        #region Mapping

        public virtual TDomainEntity MapToDomainObject(TDbEntity dbEntity)
        {
            if (dbEntity == null)
                return default;  //throw new CommonException.EntityNotFoundInDbContextException("");

            var domainEnttiy = coreContext.New<TDomainEntity>();
            MapToDomainObject(domainEnttiy, dbEntity);
            return domainEnttiy;
        }
        public abstract void MapToDomainObject(TDomainEntity domainEntity, TDbEntity dbEntity);

        public virtual List<TDomainEntity> MapToDomainObjectList(IEnumerable<TDbEntity> dbEntities)
        {
            List<TDomainEntity> domainEntities = new List<TDomainEntity>();
            foreach (var dbEntity in dbEntities)
            {
                domainEntities.Add(MapToDomainObject(dbEntity));
            }
            return domainEntities;
        }

        public abstract void MapToDbEntity(TDomainEntity domainEntity, TDbEntity dbEntity);

        #endregion

        #region Flush

        public void FlushEntity(TDomainEntity domainEntity)
        {
            TDbEntity dbEntity = dbContext.Set<TDbEntity>().Find(domainEntity.Id);
            MapToDbEntity(domainEntity, dbEntity);
            var entry = dbContext.Entry(dbEntity);
            entry.State = EntityState.Modified;
        }

        #endregion

        #region Update

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

        #endregion

        #region Insert

        protected virtual void InsertEntity(TDbEntity dbEntity, bool forceToInsertDb = true)
        {
            coreContext.Logger.Info("TDbEntity inserting to context..", dbEntity);
            dbContext.Set<TDbEntity>().Add(dbEntity);
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
            coreContext.Logger.Info(string.Format("Type of {0} entity will be inserted. forceToInsertDb: {1}", 
                domainEntity.GetType().Name, forceToInsertDb), string.Format("Db context Guid: {0}", dbContext.Guid));
            TDbEntity dbEntity = coreContext.New<TDbEntity>();
            InsertEntity(domainEntity, dbEntity, forceToInsertDb);
        }

        #endregion

        #region Delete
        public void DeleteEntity(TDomainEntity domainEntity)
        {
            var dbEntity = dbContext.Set<TDbEntity>().Find(domainEntity.Id);
            dbContext.Set<TDbEntity>().Remove(dbEntity);
        }

        public void DeleteEntity(int id)
        {
            var dbEntity = dbContext.Set<TDbEntity>().Find(id);
            dbContext.Set<TDbEntity>().Remove(dbEntity);
        }
        #endregion

        #region Data retrieving

        protected abstract TDbEntity GetDbEntityById(int id);

        public abstract TDomainEntity GetById(int id);

        public List<TDomainEntity> GetAll()
        {
            return MapToDomainObjectList(dbContext.Set<TDbEntity>().ToList());
        }

        protected TDomainEntity GetFirstBy(Expression<Func<TDbEntity, bool>> predicate)
        {
            return MapToDomainObject(dbContext.Set<TDbEntity>().FirstOrDefault(predicate));
        }

        protected List<TDomainEntity> GetListBy(Expression<Func<TDbEntity, bool>> predicate)
        {
            return MapToDomainObjectList(dbContext.Set<TDbEntity>().Where(predicate));
        }

        protected bool Exists(Expression<Func<TDbEntity, bool>> predicate)
        {
            return dbContext.Set<TDbEntity>().Any(predicate);
        }

        #endregion
    }
}
