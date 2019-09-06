using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using System.Collections.Generic;

namespace AydoganFBank.AccountManagement.Service
{
    public interface IPersonManager
    {
        IPersonInfo CreatePerson(string firstName, string lastName, string emailAddress, string identityNumber);
        IPersonInfo ChangePersonLastName(int personId, string lastName);
        IPersonInfo ChangePersonEmailAddress(int personId, string emailAddress);
        IPersonInfo GetPersonInfo(int personId);
        IPersonInfo GetPersonByIdentityNumber(string identityNumber);
        List<IPersonInfo> GetAllPersonList();
    }
}
