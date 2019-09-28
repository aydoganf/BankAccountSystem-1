using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ITokenInfo
    {
        int Id { get; }
        string Token { get; }
        DateTime ValidUntil { get; }
        int ApplicationId { get; }
        bool IsValid { get; }
        IPersonInfo PersonInfo { get; }
    }
}
