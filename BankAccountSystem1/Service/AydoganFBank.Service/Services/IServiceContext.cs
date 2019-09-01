using AydoganFBank.Service.Services.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Services
{
    public interface IServiceContext
    {
        IAccountManagerService AccountManagerService { get; }
        IPersonManagerService PersonManagerService { get; }
        ICompanyManagerService CompanyManagerService { get; }
        ICreditCardManagerService CreditCardManagerService { get; }
    }
}
