using AydoganFBank.Common.Repository;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Common.IoC
{
    public interface ICoreContext
    {
        AydoganFBankDbContext DBContext { get; }

        T New<T>();

        T Query<T>() where T : IQueryRepository;

        //ICoreContext WithNewContext();
    }
}
