using System;
using AydoganFBank.Database;
using StructureMap;

namespace AydoganFBank.Common.IoC
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

        public AydoganFBankDbContext DBContext => dbContext;

        public CoreContext(IContext context)
        {
            this.context = context;
        }

        public T New<T>()
        {
            return context.GetInstance<T>();
        }

        public T Query<T>()
        {
            return context.GetInstance<T>();
        }

        //public ICoreContext WithNewContext()
        //{
        //    return context.GetInstance<ICoreContext>();
        //}
    }
}
