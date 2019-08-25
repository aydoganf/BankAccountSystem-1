﻿using AydoganFBank.AccountManagement.Api;
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
        private readonly AccountManager accountManager;

        public CompanyManager(
            ICoreContext coreContext, PersonManager personManager, AccountManager accountManager)
        {
            this.coreContext = coreContext;
            this.personManager = personManager;
            this.accountManager = accountManager;
        }
        #endregion

        private CompanyDomainEntity CreateCompany(
            string companyName,
            PersonDomainEntity responsablePerson,
            string address,
            string phoneNumber,
            string taxNumber,
            AccountDomainEntity account)
        {
            var company = coreContext.New<CompanyDomainEntity>()
                .With(companyName, responsablePerson, address, phoneNumber, taxNumber, account);

            company.Insert();
            return company;
        }


        internal CompanyDomainEntity CreateCompany(
            string companyName,
            int responsablePersonId,
            string address,
            string phoneNumber,
            string taxNumber,
            int accountId)
        {
            var person = personManager.GetPersonById(responsablePersonId);
            var account = accountManager.GetAccountById(accountId);
            return CreateCompany(companyName, person, address, phoneNumber, taxNumber, account);
        }

        internal CompanyDomainEntity GetCompanyById(int companyId)
        {
            return coreContext.Query<ICompanyRepository>().GetById(companyId);
        }

        internal CompanyDomainEntity GetCompanyByResponsableId(int responsablePersonId)
        {
            var person = personManager.GetPersonById(responsablePersonId);
            return person.GetResponsableCompany();
        }

        internal CompanyDomainEntity GetCompanyByResponsableIdentityNumber(string responsableIdentityNumber)
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

        #region API Implementations

        ICompanyInfo ICompanyManager.CreateCompany(string companyName, int responsablePersonId, string address, string phoneNumber, string taxNumber, int accountId)
        {
            return CreateCompany(companyName, responsablePersonId, address, phoneNumber, taxNumber, accountId);
        }

        ICompanyInfo ICompanyManager.GetCompanyInfo(int companyId)
        {
            return GetCompanyById(companyId);
        }

        ICompanyInfo ICompanyManager.GetCompanyByResponsableId(int responsablePersonId)
        {
            return GetCompanyByResponsableId(responsablePersonId);
        }

        ICompanyInfo ICompanyManager.GetCompanyByResponsableIdentityNumber(string responsableIdentityNumber)
        {
            return GetCompanyByResponsableIdentityNumber(responsableIdentityNumber);
        }

        ICompanyInfo ICompanyManager.ChangeCompanyAddress(int companyId, string address)
        {
            return ChangeCompanyAddress(companyId, address);
        }

        ICompanyInfo ICompanyManager.ChangeCompanyPhoneNumber(int companyId, string phoneNumber)
        {
            return ChangeCompanyPhoneNumber(companyId, phoneNumber);
        }

        ICompanyInfo ICompanyManager.GetCompanyByTaxNumber(string taxNumber)
        {
            return GetCompanyByTaxNumber(taxNumber);
        }
        #endregion
    }
}
