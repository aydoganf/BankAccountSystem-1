using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Service.Builder;
using AydoganFBank.Service.Message.Data;
using AydoganFBank.Service.Services.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Services
{
    public class PersonManagerService : IPersonManagerService
    {
        private readonly IPersonManager personManager;
        private readonly ServiceDataBuilder dataBuilder;

        public PersonManagerService(IPersonManager personManager, ServiceDataBuilder dataBuilder)
        {
            this.personManager = personManager;
            this.dataBuilder = dataBuilder;
        }

        public PersonInfo ChangePersonEmailAddress(int personId, string emailAddress)
        {
            var person = personManager.ChangePersonEmailAddress(personId, emailAddress);
            return dataBuilder.BuildPersonInfo(person);
        }

        public PersonInfo ChangePersonLastName(int personId, string lastName)
        {
            var person = personManager.ChangePersonLastName(personId, lastName);
            return dataBuilder.BuildPersonInfo(person);
        }

        public PersonInfo CreatePerson(string firstName, string lastName, string emailAddress, string identityNumber)
        {
            var person = personManager.CreatePerson(
                firstName, lastName, emailAddress, identityNumber);
            return dataBuilder.BuildPersonInfo(person);
        }

        public PersonInfo GetPersonByIdentityNumber(string identityNumber)
        {
            var person = personManager.GetPersonByIdentityNumber(identityNumber);
            return dataBuilder.BuildPersonInfo(person);
        }

        public PersonInfo GetPersonInfo(int personId)
        {
            var person = personManager.GetPersonInfo(personId);
            return dataBuilder.BuildPersonInfo(person);
        }
    }
}
