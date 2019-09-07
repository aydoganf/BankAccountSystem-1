using AydoganFBank.Context.IoC.Lifecycle;
using System;
using System.Collections.Generic;

namespace AydoganFBank.Context.IoC
{
    public interface ICoreContextConfigurer : ISingletonObject
    {
        string GetConnectionString();
        string GetLogFileDirectory();
    }
}
