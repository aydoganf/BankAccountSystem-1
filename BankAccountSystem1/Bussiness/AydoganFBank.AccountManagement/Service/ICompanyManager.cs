using AydoganFBank.AccountManagement.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Service
{
    public interface ICompanyManager
    {
        ICompanyInfo CreateCompany(string companyName, int responsablePersonId, string address, string phoneNumber, string taxNumber, int accountId);
        ICompanyInfo GetCompanyInfo(int companyId);
        ICompanyInfo GetCompanyByResponsableId(int responsablePersonId);
        ICompanyInfo GetCompanyByResponsableIdentityNumber(string responsableIdentityNumber);
        ICompanyInfo ChangeCompanyAddress(int companyId, string address);
        ICompanyInfo ChangeCompanyPhoneNumber(int companyId, string phoneNumber);
        ICompanyInfo GetCompanyByTaxNumber(string taxNumber);
    }
}
