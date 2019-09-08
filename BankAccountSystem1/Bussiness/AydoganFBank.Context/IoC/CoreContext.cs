using System;
using System.Collections.Generic;
using System.Linq;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Context.Utils;
using AydoganFBank.Database;
using StructureMap;

namespace AydoganFBank.Context.IoC
{
    public class CoreContext : ICoreContext
    {
        private Dictionary<Type, List<IDomainEntity>> payload = new Dictionary<Type, List<IDomainEntity>>();

        private readonly IContainer container;
        private readonly ICoreContextConfigurer coreContextConfigurer;
        private readonly ICryptographer cryptographer;

        private Logger logger;

        private AydoganFBankDbContext _dbContext;
        private AydoganFBankDbContext dbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = new AydoganFBankDbContext(coreContextConfigurer.GetConnectionString());
                    //_dbContext.Database.Connection.ConnectionString = this.coreContextConfigurer.GetConnectionString();
                }
                return _dbContext;
            }
        }

        AydoganFBankDbContext ICoreContext.DBContext => dbContext;
        ILogger ICoreContext.Logger => logger;
        ICryptographer ICoreContext.Cryptographer => cryptographer;


        public CoreContext(
            IContainer container, ICoreContextConfigurer coreContextConfigurer, ICryptographer cryptographer)
        {
            this.container = container;
            this.coreContextConfigurer = coreContextConfigurer;
            this.cryptographer = cryptographer;

            logger = new Logger();
            logger.SetFilePaths(this.coreContextConfigurer.GetLogFileDirectory());

            logger.Info("coreContext created.", this);
            logger.Info("logger created.");

        }

        public T New<T>()
        {
            var obj = container.GetInstance<T>();
            if (obj.GetType().GetInterfaces().Any(@interface => @interface.Name == typeof(IDomainEntity).Name))
            {
                if(payload.ContainsKey(obj.GetType()) == false)
                {
                    payload.Add(obj.GetType(), new List<IDomainEntity>() { (IDomainEntity)obj });
                }
                else
                {
                    if (payload[obj.GetType()] == null)
                        payload[obj.GetType()] = new List<IDomainEntity>();
                    payload[obj.GetType()].Add((IDomainEntity)obj);
                }
            }

            return obj;
        }

        public T Query<T>() where T : IQueryRepository
        {
            return container.GetInstance<T>();
        }

        public string GetContainerInfo()
        {
            return container.Name;
        }

        public void Commit()
        {
            dbContext.ChangeTracker.Entries()
                .Where(e => e.State == System.Data.Entity.EntityState.Added || e.State == System.Data.Entity.EntityState.Modified)
                .ToList()
                .ForEach(a =>
            {
                logger.Info(a.State.ToString(), a.Entity);
            });
            logger.Info("Committing context..", string.Format("Db context guid: {0}", dbContext.Guid));
            dbContext.SaveChanges();
        }

    }
}
