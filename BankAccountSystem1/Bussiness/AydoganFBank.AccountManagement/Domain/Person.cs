using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.Exception;
using AydoganFBank.Context.IoC;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Database;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
{

    public class PersonDomainEntity : IDomainEntity, IAccountOwner, IPersonInfo
    {
        #region IoC
        private readonly IPersonRepository personRepository;
        private readonly ICoreContext coreContext;

        public PersonDomainEntity(
            IPersonRepository personRepository, ICoreContext coreContext)
        {
            this.coreContext = coreContext;
            this.personRepository = personRepository;
        }
        #endregion

        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string IdentityNumber { get; set; }

        AccountOwnerType IAccountOwner.OwnerType => AccountOwnerType.Person;
        int IAccountOwner.OwnerId => PersonId;
        string IAccountOwner.DisplayName => FullName;

        int IDomainEntity.Id => PersonId;

        #region Calculated properties
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
        #endregion

        public PersonDomainEntity With(
            string firstName, 
            string lastName, 
            string emailAddress, 
            string identityNumber)
        {
            FirstName = firstName ?? throw new CommonException.RequiredParameterMissingException(nameof(firstName));
            LastName = lastName ?? throw new CommonException.RequiredParameterMissingException(nameof(lastName));
            EmailAddress = emailAddress ?? throw new CommonException.RequiredParameterMissingException(nameof(emailAddress));
            IdentityNumber = identityNumber ?? throw new CommonException.RequiredParameterMissingException(nameof(identityNumber));
            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            var person = personRepository.GetByIdentityNumber(IdentityNumber);
            if (person != null)
                throw new AccountManagementException.PersonAlreadyExistWithTheGivenIdentityNumberException(string.Format("{0} = {1}", nameof(IdentityNumber), IdentityNumber));

            personRepository.InsertEntity(this, forceToInsertDb);
        }

        public void SetLastname(string lastName)
        {
            LastName = lastName;
            Save();
        }

        public void SetEmail(string emailAddress)
        {
            EmailAddress = emailAddress;
            Save();
        }

        public void Save()
        {
            personRepository.UpdateEntity(this);
        }

        public CompanyDomainEntity GetResponsableCompany()
        {
            return coreContext.Query<ICompanyRepository>().GetByResponsablePerson(this);
        }
    }

    public class PersonRepository : OrderedQueryRepository<PersonDomainEntity, Person>, IPersonRepository
    {
        public PersonRepository(
            ICoreContext coreContext,
            IDomainEntityBuilder<PersonDomainEntity, Person> domainEntityBuilder,
            IDbEntityMapper<Person, PersonDomainEntity> dbEntityMapper) 
            : base (coreContext, domainEntityBuilder, dbEntityMapper)
        {            
        }

        #region Mapping overrides
        public override void MapToDbEntity(PersonDomainEntity domainEntity, Person dbEntity)
        {
            dbEntity.EmailAddress = domainEntity.EmailAddress;
            dbEntity.FirstName = domainEntity.FirstName;
            dbEntity.LastName = domainEntity.LastName;
        }

        public override void MapToDomainObject(PersonDomainEntity domainEntity, Person dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.PersonId = dbEntity.PersonId;
            domainEntity.EmailAddress = dbEntity.EmailAddress;
            domainEntity.FirstName = dbEntity.FirstName;
            domainEntity.LastName = dbEntity.LastName;
            domainEntity.IdentityNumber = dbEntity.IdentityNumber;
        }
        #endregion

        protected override Person GetDbEntityById(int id)
        {
            return dbContext.Person.FirstOrDefault(p => p.PersonId == id);
        }

        public override PersonDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        public PersonDomainEntity GetByIdentityNumber(string identityNumber)
        {
            return GetFirstBy(
                p => p.IdentityNumber == identityNumber);
        }
    }

    public interface IPersonRepository : IRepository<PersonDomainEntity>
    {
        PersonDomainEntity GetByIdentityNumber(string identityNumber);
    }
}
