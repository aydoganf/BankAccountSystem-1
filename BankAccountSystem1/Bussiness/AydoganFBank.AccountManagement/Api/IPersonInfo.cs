using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Api
{
    public interface IPersonInfo
    {
        int Id { get; }
        string FirstName { get; }
        string LastName { get; }
        string EmailAddress { get; }
        string IdentityNumber { get; }
        string FullName { get; }
    }
}
