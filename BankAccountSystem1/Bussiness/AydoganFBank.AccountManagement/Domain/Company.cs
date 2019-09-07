using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.Exception;
using AydoganFBank.Context.IoC;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Database;
using System.Collections.Generic;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
{
    public class CompanyDomainEntity : IDomainEntity, IAccountOwner, ICompanyInfo
    {
        #region IoC
        private readonly ICompanyRepository companyRepository;
        private readonly ICoreContext coreContext;

        public CompanyDomainEntity(ICoreContext coreContext, ICompanyRepository companyRepository)
        {
            this.coreContext = coreContext;
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
        string IAccountOwner.DisplayName => CompanyName;

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
            if (company != default)
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

        public List<AccountDomainEntity> GetAccounts()
        {
            return coreContext.Query<IAccountRepository>().GetListByOwner(this);
        }

        #region Api
        int ICompanyInfo.Id => CompanyId;
        string ICompanyInfo.CompanyName => CompanyName;
        IPersonInfo ICompanyInfo.ResponsablePerson => ResponsablePerson;
        string ICompanyInfo.Address => Address;
        string ICompanyInfo.PhoneNumber => PhoneNumber;
        string ICompanyInfo.TaxNumber => TaxNumber;
        #endregion
    }

    public class CompanyRepository : Repository<CompanyDomainEntity, Company>, ICompanyRepository
    {
        public CompanyRepository(
            ICoreContext coreContext) 
            : base(coreContext)
        {
        }

        #region Mapping overrides
        public override void MapToDbEntity(CompanyDomainEntity domainEntity, Company dbEntity)
        {
            dbEntity.Address = domainEntity.Address;
            dbEntity.CompanyName = domainEntity.CompanyName;
            dbEntity.PhoneNumber = domainEntity.PhoneNumber;
            dbEntity.ResponsablePersonId = domainEntity.ResponsablePerson.PersonId;
            dbEntity.TaxNumber = domainEntity.TaxNumber;
        }

        public override void MapToDomainObject(CompanyDomainEntity domainEntity, Company dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.Address = dbEntity.Address;
            domainEntity.CompanyId = dbEntity.CompanyId;
            domainEntity.CompanyName = dbEntity.CompanyName;
            domainEntity.PhoneNumber = dbEntity.PhoneNumber;
            domainEntity.ResponsablePerson = coreContext.Query<IPersonRepository>().GetById(dbEntity.ResponsablePersonId);
            domainEntity.TaxNumber = dbEntity.TaxNumber;
        }
        #endregion

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
                dbContext.Company.Where(c => c.CompanyName.Contains(companyName)));
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
