using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Context.IoC
{
    public interface ICoreContextConfigurer
    {
        string GetConnectionString();
    }
}
