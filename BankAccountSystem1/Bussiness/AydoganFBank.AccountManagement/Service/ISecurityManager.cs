using AydoganFBank.AccountManagement.Api;

namespace AydoganFBank.AccountManagement.Service
{
    public interface ISecurityManager
    {
        ITokenInfo GetTokenInfo(int tokenId);
        ITokenInfo GetTokenByValue(string value);
        ITokenInfo CreateToken(int personId, int applicationId);
        ITokenInfo LoginByEmail(string email, string password, int applicationId);

        IApplicationInfo GetApplicationInfo(int applicationId);
        IApplicationInfo CreateApplication(string name, string domain);

    }
}
