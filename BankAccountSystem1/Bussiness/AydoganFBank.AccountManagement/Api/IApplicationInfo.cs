using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Api
{
    public interface IApplicationInfo
    {
        int Id { get; }
        string Name { get; }
        string Domain { get; }
        Guid Guid { get; }
    }
}
