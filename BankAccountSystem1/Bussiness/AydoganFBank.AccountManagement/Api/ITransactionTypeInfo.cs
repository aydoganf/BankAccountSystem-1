using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ITransactionTypeInfo
    {
        int TypeId { get; set; }
        string TypeKey { get; set; }
        string TypeName { get; set; }
    }
}
