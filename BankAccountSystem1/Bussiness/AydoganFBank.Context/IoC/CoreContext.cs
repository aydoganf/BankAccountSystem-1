using System;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Context.Utils;
using AydoganFBank.Database;
using StructureMap;

namespace AydoganFBank.Context.IoC
{
    public class CoreContext : ICoreContext
    {
        private readonly IContainer container;
        private readonly ICoreContextConfigurer coreContextConfigurer;
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

        public CoreContext(
            IContainer container, ICoreContextConfigurer coreContextConfigurer)
        {
            this.container = container;
            this.coreContextConfigurer = coreContextConfigurer;

            logger = new Logger();
            logger.SetFilePaths(this.coreContextConfigurer.GetLogFileDirectory());

            logger.Info("coreContext created.", this);
            logger.Info("logger created.");
        }

        public T New<T>()
        {
            return container.GetInstance<T>();
        }

        public T Query<T>() where T : IQueryRepository
        {
            return container.GetInstance<T>();
        }

        public string GetContainerInfo()
        {
            return container.Name;
        }

        //public ICoreContext WithNewContext()
        //{
        //    return context.GetInstance<ICoreContext>();
        //}
    }
}
