using AydoganFBank.Context.DataAccess;
using AydoganFBank.Context.IoC.Lifecycle;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Context.IoC
{
    public interface ICoreContext : ITransientObject
    {
        AydoganFBankDbContext DBContext { get; }

        T New<T>();

        T Query<T>() where T : IQueryRepository;

        //ICoreContext WithNewContext();
    }
}
