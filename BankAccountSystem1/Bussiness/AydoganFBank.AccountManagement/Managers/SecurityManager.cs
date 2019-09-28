using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Context.IoC;

namespace AydoganFBank.AccountManagement.Managers
{
    public class SecurityManager : ISecurityManager
    {
        private readonly ICoreContext coreContext;
        private readonly PersonManager personManager;

        public SecurityManager(ICoreContext coreContext, PersonManager personManager)
        {
            this.coreContext = coreContext;
            this.personManager = personManager;
        }

        internal ApplicationDomainEntity GetApplicationById(int applicationId)
        {
            return coreContext.Query<IApplicationRepository>().GetById(applicationId);
        }

        internal TokenDomainEntity CreateToken(PersonDomainEntity person, ApplicationDomainEntity application)
        {
            var token = coreContext.New<TokenDomainEntity>().With(person, application);
            token.Insert();
            return token;
        }

        internal TokenDomainEntity CreateToken(int personId, int applicationId)
        {
            var person = personManager.GetPersonById(personId);
            var application = GetApplicationById(applicationId);
            return CreateToken(person, application);
        }

        internal ApplicationDomainEntity CreateApplication(string name, string domain)
        {
            var application = coreContext.New<ApplicationDomainEntity>().With(name, domain);
            application.Insert();
            return application;
        }

        internal TokenDomainEntity LoginByEmail(string email, string passwordSalt, int applicationId)
        {
            string password = coreContext.Cryptographer.GenerateMD5Hash(passwordSalt);
            var person = personManager.GetPersonByEmailAndPassword(email, password);
            var application = GetApplicationById(applicationId);

            if (person == null)
                throw new AccountManagementException.PersonCouldNotFoundWithGivenEmailAndPassword(string.Empty);

            return CreateToken(person, application);
        }


        ITokenInfo ISecurityManager.CreateToken(int personId, int applicationId)
        {
            return CreateToken(personId, applicationId);
        }

        ITokenInfo ISecurityManager.GetTokenInfo(int tokenId)
        {
            return coreContext.Query<ITokenRepository>().GetById(tokenId);
        }

        ITokenInfo ISecurityManager.GetTokenByValue(string value)
        {
            return coreContext.Query<ITokenRepository>().GetByValue(value);
        }

        IApplicationInfo ISecurityManager.GetApplicationInfo(int applicationId)
        {
            return GetApplicationById(applicationId);
        }

        IApplicationInfo ISecurityManager.CreateApplication(string name, string domain)
        {
            return CreateApplication(name, domain);
        }

        ITokenInfo ISecurityManager.LoginByEmail(string email, string password, int applicationId)
        {
            return LoginByEmail(email, password, applicationId);
        }
    }
}
