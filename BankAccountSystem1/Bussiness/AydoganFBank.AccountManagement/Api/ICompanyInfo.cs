using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ICompanyInfo
    {
        int Id { get; }
        string CompanyName { get; }
        IPersonInfo ResponsablePerson { get; }
        string Address { get; }
        string PhoneNumber { get; }
        string TaxNumber { get; }
        IAccountInfo Account { get; }
    }
}
