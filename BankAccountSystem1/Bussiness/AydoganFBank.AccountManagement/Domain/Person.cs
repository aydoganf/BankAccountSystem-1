using AydoganFBank.Common;
using AydoganFBank.Common.Builders;
using AydoganFBank.Database;
using System.Data.Entity;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
{
    public class PersonDomainEntity : IDomainEntity
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string IdentityNumber { get; set; }
    }

    public class PersonRepository : Repository<PersonDomainEntity>
    {
        private readonly AydoganFBankDbContext dbContext;
        private readonly IDomainEntityBuilder<PersonDomainEntity, Person> personDomainObjectBuilder;

        public PersonRepository(
            AydoganFBankDbContext dbContext,
            IDomainEntityBuilder<PersonDomainEntity, Person> personDomainObjectBuilder) : base(dbContext)
        {
            this.dbContext = dbContext;
            this.personDomainObjectBuilder = personDomainObjectBuilder;
        }

        public override IQueryable<PersonDomainEntity> GetAll()
        {
            return dbContext.Person.Select(p => personDomainObjectBuilder.MapToDomainObject(p));
        }
    }

    public abstract class Repository<TDomainEntity> where TDomainEntity : IDomainEntity
    {
        private readonly AydoganFBankDbContext dbContext;

        public Repository(AydoganFBankDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public abstract IQueryable<TDomainEntity> GetAll();
    }
}
