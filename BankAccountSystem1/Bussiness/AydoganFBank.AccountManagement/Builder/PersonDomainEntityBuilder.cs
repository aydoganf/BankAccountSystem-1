using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.Common.Builders;
using AydoganFBank.Common.IoC;
using AydoganFBank.Database;
using System.Collections.Generic;

namespace AydoganFBank.AccountManagement.Builder
{
    internal class PersonDomainEntityBuilder : IDomainEntityBuilder<PersonDomainEntity, Person>
    {
        private readonly ICoreContext coreContext;

        public PersonDomainEntityBuilder(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }

        public PersonDomainEntity MapToDomainObject(Person entity)
        {
            if (entity == null)
                return null;

            var domainEntity = coreContext.New<PersonDomainEntity>();
            MapToDomainObject(domainEntity, entity);
            return domainEntity;
        }

        public void MapToDomainObject(PersonDomainEntity domainEntity, Person entity)
        {
            if (domainEntity == null || entity == null)
                return;

            domainEntity.PersonId = entity.PersonId;
            domainEntity.EmailAddress = entity.EmailAddress;
            domainEntity.FirstName = entity.FirstName;
            domainEntity.LastName = entity.LastName;
            domainEntity.IdentityNumber = entity.IdentityNumber;
        }

        public IEnumerable<PersonDomainEntity> MapToDomainObjectList(IEnumerable<Person> entities)
        {
            List<PersonDomainEntity> people = new List<PersonDomainEntity>();
            foreach (var entity in entities)
            {
                people.Add(MapToDomainObject(entity));
            }
            return people;
        }
    }

    internal class PersonMapper : IDbEntityMapper<Person, PersonDomainEntity>
    {
        public void MapToDbEntity(PersonDomainEntity domainEntity, Person dbEntity)
        {
            dbEntity.EmailAddress = domainEntity.EmailAddress;
            dbEntity.FirstName = domainEntity.FirstName;
            dbEntity.LastName = domainEntity.LastName;
        }
    }
}
