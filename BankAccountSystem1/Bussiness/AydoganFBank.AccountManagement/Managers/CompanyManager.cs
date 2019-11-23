using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Context.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Managers
{
    public class CompanyManager : ICompanyManager
    {
        #region IoC
        private readonly ICoreContext coreContext;
        private readonly PersonManager personManager;

        public CompanyManager(
            ICoreContext coreContext, PersonManager personManager)
        {
            this.coreContext = coreContext;
            this.personManager = personManager;
        }
        #endregion

        private CompanyDomainEntity CreateCompany(
            string companyName,
            PersonDomainEntity responsablePerson,
            string address,
            string phoneNumber,
            string taxNumber)
        {
            var company = coreContext.New<CompanyDomainEntity>()
                .With(companyName, responsablePerson, address, phoneNumber, taxNumber);

            company.Insert();
            return company;
        }


        internal CompanyDomainEntity CreateCompany(
            string companyName,
            int responsablePersonId,
            string address,
            string phoneNumber,
            string taxNumber)
        {
            var person = personManager.GetPersonById(responsablePersonId);
            return CreateCompany(companyName, person, address, phoneNumber, taxNumber);
        }

        internal CompanyDomainEntity GetCompanyById(int companyId)
        {
            return coreContext.Query<ICompanyRepository>().GetById(companyId);
        }

        internal List<CompanyDomainEntity> GetCompanyByResponsableId(int responsablePersonId)
        {
            var person = personManager.GetPersonById(responsablePersonId);
            return person.GetResponsableCompany();
        }

        internal List<CompanyDomainEntity> GetCompanyByResponsableIdentityNumber(string responsableIdentityNumber)
        {
            var person = personManager.GetPersonByIdentityNumber(responsableIdentityNumber);
            return person.GetResponsableCompany();
        }

        internal CompanyDomainEntity ChangeCompanyAddress(int companyId, string address)
        {
            var company = GetCompanyById(companyId);
            company.SetAddress(address);
            return company;
        }

        internal CompanyDomainEntity ChangeCompanyPhoneNumber(int companyId, string phoneNumber)
        {
            var company = GetCompanyById(companyId);
            company.SetPhoneNumber(phoneNumber);
            return company;
        }

        internal CompanyDomainEntity GetCompanyByTaxNumber(string taxNumber)
        {
            return coreContext.Query<ICompanyRepository>().GetByTaxNumber(taxNumber);
        }

        internal List<AccountDomainEntity> GetCompanyAccounts(int companyId)
        {
            var company = coreContext.Query<ICompanyRepository>().GetById(companyId);
            return company.GetAccounts();
        }


        #region API Implementations

        ICompanyInfo ICompanyManager.CreateCompany(
            string companyName,
            int responsablePersonId,
            string address,
            string phoneNumber,
            string taxNumber) 
            => CreateCompany(companyName, responsablePersonId, address, phoneNumber, taxNumber);

        ICompanyInfo ICompanyManager.GetCompanyInfo(int companyId) 
            => GetCompanyById(companyId);

        List<ICompanyInfo> ICompanyManager.GetCompanyByResponsableId(int responsablePersonId) 
            => GetCompanyByResponsableId(responsablePersonId).Cast<ICompanyInfo>().ToList();

        List<ICompanyInfo> ICompanyManager.GetCompanyByResponsableIdentityNumber(string responsableIdentityNumber) 
            => GetCompanyByResponsableIdentityNumber(responsableIdentityNumber).Cast<ICompanyInfo>().ToList();

        ICompanyInfo ICompanyManager.ChangeCompanyAddress(int companyId, string address) 
            => ChangeCompanyAddress(companyId, address);

        ICompanyInfo ICompanyManager.ChangeCompanyPhoneNumber(int companyId, string phoneNumber) 
            => ChangeCompanyPhoneNumber(companyId, phoneNumber);

        ICompanyInfo ICompanyManager.GetCompanyByTaxNumber(string taxNumber) 
            => GetCompanyByTaxNumber(taxNumber);

        List<IAccountInfo> ICompanyManager.GetCompanyAccounts(int companyId) 
            => GetCompanyAccounts(companyId).Cast<IAccountInfo>().ToList();
        #endregion
    }
}
