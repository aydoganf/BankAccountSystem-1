using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context.DataAccess;

namespace AydoganFBank.AccountManagement.Service
{
    public interface ISecurityManager : IDomianEntityManager
    {
        ITokenInfo GetTokenInfo(int tokenId);
        ITokenInfo GetTokenByValue(string value);
        ITokenInfo GetTokenByValueAndApplication(string value, int applicationId);
        ITokenInfo CreateToken(int personId, int applicationId);
        ITokenInfo LoginByEmail(string email, string password, int applicationId);

        IApplicationInfo GetApplicationInfo(int applicationId);
        IApplicationInfo GetApplicationByToken(string token);
        IApplicationInfo CreateApplication(string name, string domain);

    }
}
