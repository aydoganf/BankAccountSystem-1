using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Repository;
using AydoganFBank.Common;
using AydoganFBank.Common.Builders;
using AydoganFBank.Common.IoC;
using AydoganFBank.Database;
using System.Collections.Generic;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
{

    public class PersonDomainEntity : IDomainEntity, IAccountOwner
    {
        #region IoC
        private readonly IPersonRepository personRepository;

        public PersonDomainEntity(IPersonRepository personRepository)
        {
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

        int IDomainEntity.Id => PersonId;

        public PersonDomainEntity With(
            string firstName, 
            string lastName, 
            string emailAddress, 
            string identityNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            IdentityNumber = identityNumber;
            Insert();
            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            personRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            personRepository.UpdateEntity(this);
        }
    }

    public class PersonRepository : 
        OrderedQueryRepository<PersonDomainEntity, Person>, 
        IPersonRepository
    {
        public PersonRepository(
            ICoreContext coreContext,
            AydoganFBankDbContext dbContext,
            IDomainEntityBuilder<PersonDomainEntity, Person> domainEntityBuilder,
            IDbEntityMapper<Person, PersonDomainEntity> dbEntityMapper) 
            : base (coreContext, dbContext, domainEntityBuilder, dbEntityMapper)
        {            
        }

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
