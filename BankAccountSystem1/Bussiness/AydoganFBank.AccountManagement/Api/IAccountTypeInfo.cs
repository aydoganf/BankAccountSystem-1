using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Api
{
    public interface IAccountTypeInfo
    {
        int Id { get; }
        string TypeName { get; }
        string TypeKey { get; }
        string AssetsUnit { get; }
    }
}
