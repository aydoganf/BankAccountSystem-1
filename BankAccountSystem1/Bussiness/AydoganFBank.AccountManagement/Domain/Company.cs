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

        int IDomainEntity.Id => CompanyId;

        AccountOwnerType IAccountOwner.OwnerType => AccountOwnerType.Company;
        int IAccountOwner.OwnerId => CompanyId;

        public CompanyDomainEntity With(
            string companyName, 
            PersonDomainEntity responsablePerson, 
            string address, 
            string phoneNumber)
        {
            CompanyName = companyName;
            ResponsablePerson = responsablePerson;
            Address = address;
            PhoneNumber = phoneNumber;
            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            companyRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            companyRepository.UpdateEntity(this);
        }
    }

    public class CompanyRepository : 
        Repository<CompanyDomainEntity, Company>, 
        ICompanyRepository
    {
        public CompanyRepository(
            ICoreContext coreContext, 
            AydoganFBankDbContext dbContext, 
            IDomainEntityBuilder<CompanyDomainEntity, Company> domainEntityBuilder, 
            IDbEntityMapper<Company, CompanyDomainEntity> dbEntityMapper) 
            : base(coreContext, dbContext, domainEntityBuilder, dbEntityMapper)
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
    }

    public interface ICompanyRepository : IRepository<CompanyDomainEntity>
    {
        List<CompanyDomainEntity> GetListLikeCompanyName(string companyName);
        CompanyDomainEntity GetByResponsablePerson(PersonDomainEntity person);
    }
}
