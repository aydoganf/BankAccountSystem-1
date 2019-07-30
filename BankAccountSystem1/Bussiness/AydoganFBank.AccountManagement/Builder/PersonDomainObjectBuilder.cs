using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.Common.Builders;
using AydoganFBank.Database;
using System.Collections.Generic;

namespace AydoganFBank.AccountManagement.Builder
{
    internal class PersonDomainObjectBuilder : IDomainEntityBuilder<PersonDomainEntity, Person>
    {
        public PersonDomainEntity MapToDomainObject(Person entity)
        {
            return new PersonDomainEntity()
            {
                PersonId = entity.PersonId,
                EmailAddress = entity.EmailAddress,
                FirstName = entity.FirstName,
                IdentityNumber = entity.IdentityNumber,
                LastName = entity.LastName
            };
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
}
