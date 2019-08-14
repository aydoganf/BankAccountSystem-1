using AydoganFBank.AccountManagement.Repository;
using AydoganFBank.Common;
using AydoganFBank.Common.Builders;
using AydoganFBank.Common.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AydoganFBank.AccountManagement.Domain
{
    public class CreditCardPaymentDomainEntity : IDomainEntity
    {
        #region IoC
        private readonly ICoreContext coreContext;
        private readonly ICreditCardPaymentRepository creditCardPaymentRepository;

        public CreditCardPaymentDomainEntity(
            ICoreContext coreContext,
            ICreditCardPaymentRepository creditCardPaymentRepository)
        {
            this.coreContext = coreContext;
            this.creditCardPaymentRepository = creditCardPaymentRepository;
        }
        #endregion

        public int CreditCardPaymentId { get; set; }
        public int InstalmentIndex { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime InstalmentDate { get; set; }
        public AccountTransactionDomainEntity AccountTransaction { get; set; }

        int IDomainEntity.Id => CreditCardPaymentId;


        public void Insert()
        {
            creditCardPaymentRepository.InsertEntity(this);
        }

        public void Save()
        {
            creditCardPaymentRepository.UpdateEntity(this);
        }

        public CreditCardPaymentDomainEntity With(
            int instalmentIndex, decimal amount, string description, DateTime createDate, 
            DateTime instalmentDate, AccountTransactionDomainEntity accountTransaction)
        {
            InstalmentIndex = instalmentIndex;
            Amount = amount;
            Description = description;
            CreateDate = createDate;
            InstalmentDate = instalmentDate;
            AccountTransaction = accountTransaction;

            return this;
        }
    }

    public class CreditCardPaymentRepository : 
        OrderedQueryRepository<CreditCardPaymentDomainEntity, CreditCardPayment>,
        ICreditCardPaymentRepository
    {
        public CreditCardPaymentRepository(
            ICoreContext coreContext, 
            IDomainEntityBuilder<CreditCardPaymentDomainEntity, CreditCardPayment> domainEntityBuilder, 
            IDbEntityMapper<CreditCardPayment, CreditCardPaymentDomainEntity> dbEntityMapper) 
            : base(coreContext, domainEntityBuilder, dbEntityMapper)
        {
        }

        public override CreditCardPaymentDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override CreditCardPayment GetDbEntityById(int id)
        {
            return dbContext.CreditCardPayment.Single(ccp => ccp.CreditCardPaymentId == id);
        }
    }

    public interface ICreditCardPaymentRepository : IRepository<CreditCardPaymentDomainEntity>
    {

    }
}
