using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Context.Exception;
using AydoganFBank.Context.IoC;
using AydoganFBank.Database;
using System;
using System.Linq;

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


        int IDomainEntity.Id => TokenId;

        int ITokenInfo.Id => TokenId;
        string ITokenInfo.Token => Value;
        DateTime ITokenInfo.ValidUntil => ValidUntil;
        int ITokenInfo.ApplicationId => Application.ApplicationId;
        bool ITokenInfo.IsValid => ValidUntil >= DateTime.Now;
        IPersonInfo ITokenInfo.PersonInfo => Person;

        public TokenDomainEntity With(PersonDomainEntity person, ApplicationDomainEntity application)
        {
            Person = person ?? throw new CommonException.RequiredParameterMissingException(nameof(person));
            Application = application ?? throw new CommonException.RequiredParameterMissingException(nameof(application));

            Guid guid = Guid.NewGuid();
            Value = coreContext.Cryptographer.GenerateMD5Hash(guid.ToString());

            CreateDate = DateTime.Now;
            ValidUntil = CreateDate.AddHours(1);

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

        public void SlideValidDate(int minutes = 60)
        {
            ValidUntil = ValidUntil.AddMinutes(minutes);
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
        }
        #endregion

        protected override Token GetDbEntityById(int id)
        {
            return dbContext.Token.FirstOrDefault(t => t.TokenId == id);
        }

        public TokenDomainEntity GetByValue(string value)
        {
            return GetFirstBy(
                t =>
                    t.Value == value);
        }
    }

    public interface ITokenRepository : IRepository<TokenDomainEntity>
    {
        TokenDomainEntity GetByValue(string value);
    }
}
