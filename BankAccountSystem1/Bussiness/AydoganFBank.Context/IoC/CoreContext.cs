using System;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Database;
using StructureMap;

namespace AydoganFBank.Context.IoC
{
    public class CoreContext : ICoreContext
    {
        private readonly IContext context;

        private AydoganFBankDbContext _dbContext;
        private AydoganFBankDbContext dbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = new AydoganFBankDbContext();
                }
                return _dbContext;
            }
        }

        AydoganFBankDbContext ICoreContext.DBContext => dbContext;

        public CoreContext(IContext context)
        {
            this.context = context;
        }

        public T New<T>()
        {
            return context.GetInstance<T>();
        }

        public T Query<T>() where T : IQueryRepository
        {
            return context.GetInstance<T>();
        }

        //public ICoreContext WithNewContext()
        //{
        //    return context.GetInstance<ICoreContext>();
        //}
    }
}
