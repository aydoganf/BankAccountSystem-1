using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Context.Exception;
using AydoganFBank.Context.IoC;
using AydoganFBank.Context.Query;
using AydoganFBank.Database;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace AydoganFBank.AccountManagement.Domain
{
    public class TokenDomainEntity : IDomainEntity, ITokenInfo
    {
        #region IoC
        private readonly ITokenRepository tokenRepository;
        private readonly ICoreContext coreContext;

        public TokenDomainEntity(ICoreContext coreContext, ITokenRepository tokenRepository)
        {
            this.coreContext = coreContext;
            this.tokenRepository = tokenRepository;
        }
        #endregion

        public int TokenId { get; set; }
        public string Value { get; set; }
        public PersonDomainEntity Person { get; set; }
        public DateTime ValidUntil { get; set; }
        public ApplicationDomainEntity Application { get; set; }
        public DateTime CreateDate { get; set; }
        public bool CanBeUsed { get; set; }

        #region Calculated properties
        public bool IsValid => ValidUntil >= DateTime.Now && CanBeUsed;
        #endregion

        int IDomainEntity.Id => TokenId;

        int ITokenInfo.Id => TokenId;
        string ITokenInfo.Token => Value;
        DateTime ITokenInfo.ValidUntil => ValidUntil;
        int ITokenInfo.ApplicationId => Application.ApplicationId;
        bool ITokenInfo.IsValid => IsValid;
        IPersonInfo ITokenInfo.PersonInfo => Person;

        public TokenDomainEntity With(PersonDomainEntity person, ApplicationDomainEntity application)
        {
            Person = person ?? throw new CommonException.RequiredParameterMissingException(nameof(person));
            Application = application ?? throw new CommonException.RequiredParameterMissingException(nameof(application));

            Guid guid = Guid.NewGuid();
            Value = coreContext.Cryptographer.GenerateMD5Hash(guid.ToString());

            CreateDate = DateTime.Now;
            ValidUntil = CreateDate.AddMinutes(application.TokenValidationMinute);
            CanBeUsed = true;

            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            tokenRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            tokenRepository.UpdateEntity(this);
        }

        public void SlideValidDateBy(ApplicationDomainEntity application)
        {
            ValidUntil = ValidUntil.AddMinutes(application.TokenSlidingMinute);
            Save();
        }

        public void MakeUnusable()
        {
            this.CanBeUsed = false;
            Save();
        }
    }

    public class TokenRepository : Repository<TokenDomainEntity, Token>, ITokenRepository
    {
        public TokenRepository(ICoreContext coreContext) : base(coreContext)
        {
        }

        public override TokenDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        #region Mapping overrides

        public override void MapToDbEntity(TokenDomainEntity domainEntity, Token dbEntity)
        {
            dbEntity.ApplicationId = domainEntity.Application.ApplicationId;
            dbEntity.CreateDate = domainEntity.CreateDate;
            dbEntity.PersonId = domainEntity.Person.PersonId;
            dbEntity.ValidUntil = domainEntity.ValidUntil;
            dbEntity.Value = domainEntity.Value;
            dbEntity.CanBeUsed = domainEntity.CanBeUsed;
        }

        public override void MapToDomainObject(TokenDomainEntity domainEntity, Token dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.Application = coreContext.Query<IApplicationRepository>().GetById(dbEntity.ApplicationId);
            domainEntity.CreateDate = dbEntity.CreateDate;
            domainEntity.Person = coreContext.Query<IPersonRepository>().GetById(dbEntity.PersonId);
            domainEntity.TokenId = dbEntity.TokenId;
            domainEntity.ValidUntil = dbEntity.ValidUntil;
            domainEntity.Value = dbEntity.Value;
            domainEntity.CanBeUsed = dbEntity.CanBeUsed;
        }
        #endregion

        protected override Token GetDbEntityById(int id)
        {
            return dbContext.Token.FirstOrDefault(t => t.TokenId == id);
        }

        private TokenDomainEntity Validated(Expression<Func<Token, bool>> predicate)
        {
            return GetFirstBy(predicate.CombineWithAnd(t => t.ValidUntil > DateTime.Now));
        } 

        public TokenDomainEntity ValidatedAndBy(int personId, int applicationId)
        {
            return Validated(
                t =>
                    t.PersonId == personId && t.ApplicationId == applicationId);
        }

        public TokenDomainEntity ValidatedAndBy(int personId, int applicationId, bool canBeUsed)
        {
            return Validated(
                t =>
                    t.PersonId == personId && t.ApplicationId == applicationId && t.CanBeUsed == canBeUsed);
        }

        public TokenDomainEntity ValidatedAndBy(string value)
        {
            return Validated(
                t =>
                    t.Value == value);
        }

        public TokenDomainEntity ValidatedAndBy(string value, int applicationId)
        {
            return Validated(
                t =>
                    t.Value == value && t.ApplicationId == applicationId);
        }

        public TokenDomainEntity By(string value, int applicationId)
        {
            return GetFirstBy(
                t =>
                    t.Value == value && t.ApplicationId == applicationId);
        }

        TokenDomainEntity ITokenRepository.GetPlainByValueAndApplication(string value, int applicationId) => By(value, applicationId);
        TokenDomainEntity ITokenRepository.GetByValue(string value) => ValidatedAndBy(value);
        TokenDomainEntity ITokenRepository.GetByValueAndApplication(string value, int applicationId) => ValidatedAndBy(value, applicationId);
        TokenDomainEntity ITokenRepository.GetByPersonAndApplication(int personId, int applicationId) => ValidatedAndBy(personId, applicationId);
        TokenDomainEntity ITokenRepository.GetByPersonAndApplication(int personId, int applicationId, bool canBeUsed) => ValidatedAndBy(personId, applicationId, canBeUsed);
    }

    public interface ITokenRepository : IRepository<TokenDomainEntity>
    {
        TokenDomainEntity GetPlainByValueAndApplication(string value, int applicationId);
        TokenDomainEntity GetByValue(string value);
        TokenDomainEntity GetByValueAndApplication(string value, int applicationId);
        TokenDomainEntity GetByPersonAndApplication(int personId, int applicationId);
        TokenDomainEntity GetByPersonAndApplication(int personId, int applicationId, bool canBeUsed);
    }
}
