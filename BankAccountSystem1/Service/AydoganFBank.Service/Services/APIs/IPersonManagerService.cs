using AydoganFBank.Service.Message.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Services.APIs
{
    public interface IPersonManagerService
    {
        PersonInfo GetPersonInfo(int personId);
        PersonInfo GetPersonByIdentityNumber(string identityNumber);

        PersonInfo CreatePerson(string firstName, string lastName, string emailAddress, string identityNumber);
        PersonInfo ChangePersonLastName(int personId, string lastName);
        PersonInfo ChangePersonEmailAddress(int personId, string emailAddress);
    }
}
