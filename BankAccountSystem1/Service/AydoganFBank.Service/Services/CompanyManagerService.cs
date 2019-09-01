using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Service.Builder;
using AydoganFBank.Service.Message.Data;
using AydoganFBank.Service.Services.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Services
{
    public class CompanyManagerService : ICompanyManagerService
    {
        private readonly ICompanyManager companyManager;
        private readonly ServiceDataBuilder dataBuilder;

        public CompanyManagerService(ICompanyManager companyManager, ServiceDataBuilder dataBuilder)
        {
            this.companyManager = companyManager;
            this.dataBuilder = dataBuilder;
        }

        public CompanyInfo ChangeCompanyAddress(int companyId, string address)
        {
            var company = companyManager.ChangeCompanyAddress(companyId, address);
            return dataBuilder.BuildCompanyInfo(company);
        }

        public CompanyInfo ChangeCompanyPhoneNumber(int companyId, string phoneNumber)
        {
            var company = companyManager.ChangeCompanyPhoneNumber(companyId, phoneNumber);
            return dataBuilder.BuildCompanyInfo(company);
        }

        public CompanyInfo CreateCompany(string companyName, int responsablePersonId, string address, string phoneNumber, string taxNumber, int accountId)
        {
            var company = companyManager.CreateCompany(
                companyName, responsablePersonId, address, phoneNumber, taxNumber, accountId);
            return dataBuilder.BuildCompanyInfo(company);
        }

        public CompanyInfo GetCompanyByResponsableId(int responsablePersonId)
        {
            var company = companyManager.GetCompanyByResponsableId(responsablePersonId);
            return dataBuilder.BuildCompanyInfo(company);
        }

        public CompanyInfo GetCompanyByResponsableIdentityNumber(string responsableIdentityNumber)
        {
            var company = companyManager.GetCompanyByResponsableIdentityNumber(responsableIdentityNumber);
            return dataBuilder.BuildCompanyInfo(company);
        }

        public CompanyInfo GetCompanyByTaxNumber(string taxNumber)
        {
            var company = companyManager.GetCompanyByTaxNumber(taxNumber);
            return dataBuilder.BuildCompanyInfo(company);
        }

        public CompanyInfo GetCompanyInfo(int companyId)
        {
            var company = companyManager.GetCompanyInfo(companyId);
            return dataBuilder.BuildCompanyInfo(company);
        }
    }
}
