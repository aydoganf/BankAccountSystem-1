using AydoganFBank.Service.Message.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Services.APIs
{
    public interface ICompanyManagerService
    {
        CompanyInfo CreateCompany(string companyName, int responsablePersonId, string address, string phoneNumber, string taxNumber);
        CompanyInfo GetCompanyInfo(int companyId);
        CompanyInfo GetCompanyByResponsableId(int responsablePersonId);
        CompanyInfo GetCompanyByResponsableIdentityNumber(string responsableIdentityNumber);
        CompanyInfo ChangeCompanyAddress(int companyId, string address);
        CompanyInfo ChangeCompanyPhoneNumber(int companyId, string phoneNumber);
        CompanyInfo GetCompanyByTaxNumber(string taxNumber);

        List<AccountInfo> GetAccountListByCompany(int companyId);
    }
}
