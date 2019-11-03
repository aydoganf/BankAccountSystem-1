using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Context.IoC;
using System;

namespace AydoganFBank.AccountManagement.Managers
{
    public class SecurityManager : ISecurityManager
    {
        private readonly ICoreContext coreContext;
        private readonly PersonManager personManager;
        private readonly AccountManager accountManager;
        private readonly CompanyManager companyManager;

        public SecurityManager(
            ICoreContext coreContext, 
            PersonManager personManager, 
            AccountManager accountManager,
            CompanyManager companyManager)
        {
            this.coreContext = coreContext;
            this.personManager = personManager;
            this.accountManager = accountManager;
            this.companyManager = companyManager;
        }

        internal ApplicationDomainEntity GetApplicationById(int applicationId)
        {
            return coreContext.Query<IApplicationRepository>().GetById(applicationId);
        }

        internal ApplicationDomainEntity GetApplicationByTokenValue(string token)
        {
            return coreContext.Query<IApplicationRepository>().GetByToken(token);
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

        internal TokenDomainEntity Login(string identity, string passwordSalt, int applicationId)
        {
            PersonDomainEntity person = null;
            if (identity.Length == 11)
            {
                // its identity number
                person = personManager.GetPersonByIdentityNumber(identity);

                if (person == null)
                    throw new AccountManagementException.PersonCouldNotFoundWithGivenIdentityNumber(identity);
            }
            else
            {
                // its account number
                var account = accountManager.GetAccountByAccountNumber(identity);

                if (account == null)
                    throw new AccountManagementException.AccountCouldNotFoundWithGivenAccountNumber(identity);

                if (account.AccountOwner.OwnerType == AccountOwnerType.Person)
                {
                    person = personManager.GetPersonById(account.AccountOwner.OwnerId);
                }
                else
                {
                    var company = companyManager.GetCompanyById(account.AccountOwner.OwnerId);
                    person = company.ResponsablePerson;
                }
            }

            string password = coreContext.Cryptographer.GenerateMD5Hash(passwordSalt);
            if (password != person.Password)
            {
                throw new AccountManagementException.LoginInformationIsNotValid(string.Empty);
            }

            var application = GetApplicationById(applicationId);

            var oldToken = coreContext.Query<ITokenRepository>().GetByPersonAndApplication(person.PersonId, application.ApplicationId, canBeUsed: true);
            if (oldToken != null)
            {
                oldToken.MakeUnusable();
            }

            return CreateToken(person, application);
        }

        internal TokenDomainEntity ValidateToken(string tokenValue, int applicationId)
        {
            var token = coreContext.Query<ITokenRepository>().GetPlainByValueAndApplication(tokenValue, applicationId);
            if (token == null)
                throw new AccountManagementException.TokenCouldNotFoundWithGivenInformations(tokenValue);

            if (token.Application.ApplicationId != applicationId)
                throw new AccountManagementException.TokenCouldNotFoundWithGivenInformations(tokenValue);

            if (token.IsValid == false)
                throw new AccountManagementException.TokenIsNotValid(string.Empty);

            var checkTimeInterval = (token.ValidUntil - DateTime.Now).Minutes;

            if (checkTimeInterval < 0)
                throw new AccountManagementException.TokenIsNotValid(string.Empty);

            if (checkTimeInterval <= token.Application.TokenSlidingCheckMinute)
            {
                // we will slide the token valid date by application
                token.SlideValidDateBy(token.Application);
            }

            return token;
        }

        internal TokenDomainEntity LoginByEmail(string email, string passwordSalt, int applicationId)
        {
            string password = coreContext.Cryptographer.GenerateMD5Hash(passwordSalt);
            var person = personManager.GetPersonByEmailAndPassword(email, password);

            if (person == null)
                throw new AccountManagementException.PersonCouldNotFoundWithGivenEmailAndPassword(string.Empty);

            var application = GetApplicationById(applicationId);
            return CreateToken(person, application);
        }

        internal TokenDomainEntity LoginByIdentityNumber(string identityNumber, string passwordSalt, int applicationId)
        {
            string password = coreContext.Cryptographer.GenerateMD5Hash(passwordSalt);
            var person = personManager.GetPersonByIdentityNumberAndPassword(identityNumber, password);
            
            if(person == null)
                throw new AccountManagementException.PersonCouldNotFoundWithGivenIdentityNumberAndPassword(string.Empty);

            var application = GetApplicationById(applicationId);

            return CreateToken(person, application);
        }


        #region Api Implementations

        ITokenInfo ISecurityManager.ValidateToken(string tokenValue, int applicationId) => ValidateToken(tokenValue, applicationId);

        ITokenInfo ISecurityManager.CreateToken(int personId, int applicationId) => CreateToken(personId, applicationId);

        ITokenInfo ISecurityManager.GetTokenInfo(int tokenId) => coreContext.Query<ITokenRepository>().GetById(tokenId);

        ITokenInfo ISecurityManager.GetTokenByValue(string value) => coreContext.Query<ITokenRepository>().GetByValue(value);

        ITokenInfo ISecurityManager.GetTokenByValueAndApplication(string value, int applicationId) => coreContext.Query<ITokenRepository>().GetByValueAndApplication(value, applicationId);

        IApplicationInfo ISecurityManager.GetApplicationInfo(int applicationId) => GetApplicationById(applicationId);

        IApplicationInfo ISecurityManager.CreateApplication(string name, string domain) => CreateApplication(name, domain);

        ITokenInfo ISecurityManager.LoginByEmail(string email, string password, int applicationId) => LoginByEmail(email, password, applicationId);

        ITokenInfo ISecurityManager.Login(string identity, string passwordSalt, int applicationId) => Login(identity, passwordSalt, applicationId);

        IApplicationInfo ISecurityManager.GetApplicationByToken(string token) => GetApplicationByTokenValue(token);
        #endregion
    }
}
