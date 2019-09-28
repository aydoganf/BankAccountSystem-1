using AydoganFBank.Context.DataAccess;
using AydoganFBank.Context.IoC.Lifecycle;
using AydoganFBank.Context.Utils;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Context.IoC
{
    public interface ICoreContext : ITransientObject
    {
        AydoganFBankDbContext DBContext { get; }
        ILogger Logger { get; }
        ICryptographer Cryptographer { get; }
        ISession Session { get; }

        T New<T>();

        T Query<T>() where T : IQueryRepository;

        void Commit();

        //ICoreContext WithNewContext();

        string GetContainerInfo();
        void SetSession(string token, DateTime validUntil);
    }
}
