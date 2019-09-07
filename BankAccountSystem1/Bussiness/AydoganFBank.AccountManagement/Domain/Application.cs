using AydoganFBank.Context.DataAccess;
using AydoganFBank.Context.Exception;
using AydoganFBank.Context.IoC;
using AydoganFBank.Database;
using System;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
{
    public class ApplicationDomainEntity : IDomainEntity
    {
        #region IoC
        private readonly ICoreContext coreContext;
        private readonly IApplicationRepository applicationRepository;

        public ApplicationDomainEntity(ICoreContext coreContext, IApplicationRepository applicationRepository)
        {
            this.coreContext = coreContext;
            this.applicationRepository = applicationRepository;
        }
        #endregion

        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Token { get; set; }
        public Guid Guid { get; set; }


        int IDomainEntity.Id => ApplicationId;

        public ApplicationDomainEntity With(string name, string domain)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new CommonException.RequiredParameterMissingException(nameof(name));
            Domain = !string.IsNullOrWhiteSpace(domain) ? domain : throw new CommonException.RequiredParameterMissingException(nameof(domain));
            Guid = Guid.NewGuid();
            Token = coreContext.Cryptographer.GenerateMD5Hash(Guid.ToString());

            return this;
        }

        public void Insert(bool forceToInsertDb = true)
        {
            applicationRepository.InsertEntity(this, forceToInsertDb);
        }

        public void Save()
        {
            applicationRepository.UpdateEntity(this);
        }
    }

    public class ApplicationRepository : Repository<ApplicationDomainEntity, Application>
    {
        public ApplicationRepository(ICoreContext coreContext) : base(coreContext)
        {
        }

        public override ApplicationDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        #region Mapping overrides

        public override void MapToDbEntity(ApplicationDomainEntity domainEntity, Application dbEntity)
        {
            dbEntity.Domain = domainEntity.Domain;
            dbEntity.Guid = domainEntity.Guid;
            dbEntity.Name = domainEntity.Name;
            dbEntity.Token = domainEntity.Token;
        }

        public override void MapToDomainObject(ApplicationDomainEntity domainEntity, Application dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.ApplicationId = dbEntity.ApplicationId;
            domainEntity.Domain = dbEntity.Domain;
            domainEntity.Guid = dbEntity.Guid;
            domainEntity.Name = dbEntity.Name;
            domainEntity.Token = dbEntity.Token;
        }

        #endregion


        protected override Application GetDbEntityById(int id)
        {
            return dbContext.Application.FirstOrDefault(a => a.ApplicationId == id);
        }

        public ApplicationDomainEntity GetByDomain(string domain)
        {
            return GetFirstBy(
                a =>
                    a.Domain == domain);
        }

        public ApplicationDomainEntity GetByToken(string token)
        {
            return GetFirstBy(
                a =>
                    a.Token == token);
        }
    }

    public interface IApplicationRepository : IRepository<ApplicationDomainEntity>
    {
        ApplicationDomainEntity GetByDomain(string domain);
        ApplicationDomainEntity GetByToken(string token);
    }
}
