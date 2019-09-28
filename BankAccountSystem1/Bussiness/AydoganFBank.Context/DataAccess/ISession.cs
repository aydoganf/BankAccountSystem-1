using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Context.DataAccess
{
    public interface ISession
    {
        string Token { get; }
        bool IsValid { get; }
        void SetToken(string token, DateTime validUntil);
    }
}
