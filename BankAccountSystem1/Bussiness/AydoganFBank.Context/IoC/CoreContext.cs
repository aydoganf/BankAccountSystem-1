using System;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Database;
using StructureMap;

namespace AydoganFBank.Context.IoC
{
    public class CoreContext : ICoreContext
    {
        private readonly IContainer container;
        private readonly ICoreContextConfigurer coreContextConfigurer;


        private string ConnStr = "";

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

        public CoreContext(IContainer container, ICoreContextConfigurer coreContextConfigurer)
        {
            this.container = container;
            this.coreContextConfigurer = coreContextConfigurer;
        }

        public T New<T>()
        {
            return container.GetInstance<T>();
        }

        public T Query<T>() where T : IQueryRepository
        {
            return container.GetInstance<T>();
        }

        //public ICoreContext WithNewContext()
        //{
        //    return context.GetInstance<ICoreContext>();
        //}
    }
}
