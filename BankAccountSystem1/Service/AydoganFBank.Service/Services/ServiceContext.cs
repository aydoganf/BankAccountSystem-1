using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service.Services.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Services
{
    public class ServiceContext : IServiceContext
    {
        public IAccountManagerService AccountManagerService { get; private set; }
        public IPersonManagerService PersonManagerService { get; private set; }
        public ICompanyManagerService CompanyManagerService { get; private set; }
        public ICreditCardManagerService CreditCardManagerService { get; private set; }

        public ServiceContext(
            IAccountManagerService accountManagerService, IPersonManagerService personManagerService,
            ICompanyManagerService companyManagerService, ICreditCardManagerService creditCardManagerService)
        {
            AccountManagerService = accountManagerService;
            PersonManagerService = personManagerService;
            CompanyManagerService = companyManagerService;
            CreditCardManagerService = creditCardManagerService;
        }
    }
}
