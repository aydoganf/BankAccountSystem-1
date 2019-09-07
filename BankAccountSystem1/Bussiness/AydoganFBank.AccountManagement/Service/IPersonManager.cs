using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.Context.DataAccess;
using System.Collections.Generic;

namespace AydoganFBank.AccountManagement.Service
{
    public interface IPersonManager : IDomianEntityManager
    {
        IPersonInfo CreatePerson(string firstName, string lastName, string emailAddress, string identityNumber);
        IPersonInfo ChangePersonLastName(int personId, string lastName);
        IPersonInfo ChangePersonEmailAddress(int personId, string emailAddress);
        IPersonInfo GetPersonInfo(int personId);
        IPersonInfo GetPersonByIdentityNumber(string identityNumber);
        List<IPersonInfo> GetAllPersonList();
    }
}
