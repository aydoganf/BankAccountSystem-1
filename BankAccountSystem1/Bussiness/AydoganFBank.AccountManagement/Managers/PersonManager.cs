using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Context.IoC;
using System.Collections.Generic;
using System.Linq;

namespace AydoganFBank.AccountManagement.Managers
{
    public class PersonManager : IPersonManager
    {
        #region IoC
        private readonly ICoreContext coreContext;


        public PersonManager(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }
        #endregion


        internal PersonDomainEntity CreatePerson(string firstName, string lastName, string emailAddress, string identityNumber)
        {
            var person = coreContext.New<PersonDomainEntity>()
                .With(firstName, lastName, emailAddress, identityNumber);

            person.Insert();
            return person;
        }

        internal PersonDomainEntity ChangePersonLastName(int personId, string lastName)
        {
            var person = coreContext.Query<IPersonRepository>().GetById(personId);
            person.SetLastname(lastName);
            return person;
        }

        internal PersonDomainEntity ChangePersonEmailAddress(int personId, string emailAddress)
        {
            var person = coreContext.Query<IPersonRepository>().GetById(personId);
            person.SetEmail(emailAddress);
            return person;
        }

        internal PersonDomainEntity GetPersonById(int personId)
        {
            return coreContext.Query<IPersonRepository>().GetById(personId);
        }

        internal PersonDomainEntity GetPersonByIdentityNumber(string identityNumber)
        {
            return coreContext.Query<IPersonRepository>().GetByIdentityNumber(identityNumber);
        }

        internal List<PersonDomainEntity> GetAllPersons()
        {
            return coreContext.Query<IPersonRepository>().GetAll();
        }

        #region API Implementations

        IPersonInfo IPersonManager.ChangePersonEmailAddress(int personId, string emailAddress)
        {
            return ChangePersonEmailAddress(personId, emailAddress);
        }

        IPersonInfo IPersonManager.ChangePersonLastName(int personId, string lastName)
        {
            return ChangePersonLastName(personId, lastName);
        }

        IPersonInfo IPersonManager.CreatePerson(string firstName, string lastName, string emailAddress, string identityNumber)
        {
            return CreatePerson(firstName, lastName, emailAddress, identityNumber);
        }

        IPersonInfo IPersonManager.GetPersonByIdentityNumber(string identityNumber)
        {
            return GetPersonByIdentityNumber(identityNumber);
        }

        IPersonInfo IPersonManager.GetPersonInfo(int personId)
        {
            return GetPersonById(personId);
        }

        List<IPersonInfo> IPersonManager.GetAllPersonList()
        {
            return GetAllPersons().Cast<IPersonInfo>().ToList();
        }
        #endregion
    }
}
