using AydoganFBank.Common;
using System;
using System.Collections.Generic;
using System.Text;
using AydoganFBank.AccountManagement.Repository;
using AydoganFBank.Database;
using AydoganFBank.Common.Builders;
using AydoganFBank.Common.IoC;
using System.Linq;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Common.Exception;

namespace AydoganFBank.AccountManagement.Domain
{
    public class CompanyDomainEntity : IDomainEntity, IAccountOwner
    {
        #region IoC
        private readonly ICompanyRepository companyRepository;

        public CompanyDomainEntity(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }
        #endregion

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public PersonDomainEntity ResponsablePerson { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxNumber { get; set; }

        int IDomainEntity.Id => CompanyId;

        AccountOwnerType IAccountOwner.OwnerType => AccountOwnerType.Company;
        int IAccountOwner.OwnerId => CompanyId;

        public CompanyDomainEntity With(
            string companyName, 
            PersonDomainEntity responsablePerson, 
            string address, 
            string phoneNumber,
            string taxNumber)
        {
            CompanyName = companyName ?? throw new CommonException.RequiredParameterMissingException(nameof(companyName));
            ResponsablePerson = responsablePerson ?? throw new CommonException.RequiredParameterMissingException(nameof(responsablePerson));
            Address = address ?? throw new CommonException.RequiredParameterMissingException(nameof(address));
            PhoneNumber = phoneNumber ?? throw new CommonException.RequiredParameterMissingException(nameof(phoneNumber));
            TaxNumber = taxNumber ?? throw new CommonException.RequiredParameterMissingException(nameof(taxNumber));
            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            var company = companyRepository.GetByTaxNumber(TaxNumber);
            if (company != null)
                throw new AccountManagementException.CompanyAlreadyExistWithTheGivenTaxNumberException(string.Format("{0} = {1}", nameof(TaxNumber), TaxNumber));
            companyRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            companyRepository.UpdateEntity(this);
        }

        public void SetAddress(string address)
        {
            Address = address;
            Save();
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
            Save();
        }
    }

    public class CompanyRepository : 
        Repository<CompanyDomainEntity, Company>, 
        ICompanyRepository
    {
        public CompanyRepository(
            ICoreContext coreContext,
            IDomainEntityBuilder<CompanyDomainEntity, Company> domainEntityBuilder, 
            IDbEntityMapper<Company, CompanyDomainEntity> dbEntityMapper) 
            : base(coreContext, domainEntityBuilder, dbEntityMapper)
        {
        }

        public override CompanyDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override Company GetDbEntityById(int id)
        {
            return dbContext.Company.FirstOrDefault(c => c.CompanyId == id);
        }

        public CompanyDomainEntity GetByResponsablePerson(PersonDomainEntity person)
        {
            return GetFirstBy(p => p.ResponsablePersonId == person.PersonId);
        }

        public List<CompanyDomainEntity> GetListLikeCompanyName(string companyName)
        {
            return MapToDomainObjectList(
                dbContext.Company.Where(c => c.CompanyName.Contains(companyName)))
                .ToList();
        }

        public CompanyDomainEntity GetByTaxNumber(string taxNumber)
        {
            return GetFirstBy(
                c =>
                    c.TaxNumber == taxNumber);
        }
    }

    public interface ICompanyRepository : IRepository<CompanyDomainEntity>
    {
        List<CompanyDomainEntity> GetListLikeCompanyName(string companyName);
        CompanyDomainEntity GetByResponsablePerson(PersonDomainEntity person);
        CompanyDomainEntity GetByTaxNumber(string taxNumber);
    }
}
