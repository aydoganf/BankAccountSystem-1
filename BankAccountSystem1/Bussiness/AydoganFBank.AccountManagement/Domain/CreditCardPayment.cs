using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.IoC;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
{
    public class CreditCardPaymentDomainEntity : IDomainEntity, ITransactionHolder
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

        ITransactionInfo ITransactionHolder.TransactionInfo => AccountTransaction;
        DateTime ITransactionHolder.CreateDate => CreateDate;

        #region CRUD

        public void Insert()
        {
            creditCardPaymentRepository.InsertEntity(this);
        }

        public void Save()
        {
            creditCardPaymentRepository.UpdateEntity(this);
        }
        #endregion

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

    public class CreditCardPaymentRepository : OrderedQueryRepository<CreditCardPaymentDomainEntity, CreditCardPayment>,
        ICreditCardPaymentRepository
    {
        public CreditCardPaymentRepository(
            ICoreContext coreContext) 
            : base(coreContext)
        {
        }

        #region Mapping overrides
        public override void MapToDbEntity(CreditCardPaymentDomainEntity domainEntity, CreditCardPayment dbEntity)
        {
            dbEntity.AccountTransactionId = domainEntity.AccountTransaction.TransactionId;
            dbEntity.Amount = domainEntity.Amount;
            dbEntity.CreateDate = domainEntity.CreateDate;
            dbEntity.Description = domainEntity.Description;
            dbEntity.InstalmentDate = domainEntity.InstalmentDate;
            dbEntity.InstalmentIndex = domainEntity.InstalmentIndex;
        }

        public override void MapToDomainObject(CreditCardPaymentDomainEntity domainEntity, CreditCardPayment dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.AccountTransaction = coreContext.Query<IAccountTransactionRepository>().GetById(dbEntity.AccountTransactionId);
            domainEntity.Amount = dbEntity.Amount;
            domainEntity.CreateDate = dbEntity.CreateDate;
            domainEntity.CreditCardPaymentId = dbEntity.CreditCardPaymentId;
            domainEntity.Description = dbEntity.Description;
            domainEntity.InstalmentDate = dbEntity.InstalmentDate;
            domainEntity.InstalmentIndex = dbEntity.InstalmentIndex;
        }
        #endregion

        public override CreditCardPaymentDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override CreditCardPayment GetDbEntityById(int id)
        {
            return dbContext.CreditCardPayment.Single(ccp => ccp.CreditCardPaymentId == id);
        }

        public List<CreditCardPaymentDomainEntity> GetListByCreditCard(CreditCardDomainEntity creditCard, DateTime fromDate)
        {
            var transactions = coreContext.Query<IAccountTransactionRepository>()
                .GetLastOutgoingDateRangeListByTransactionOwner(creditCard, fromDate, DateTime.Now);

            return GetOrderedDescListBy(
                ccp =>
                    transactions.Select(t => t.TransactionId).Contains(ccp.AccountTransactionId),
                ccp =>
                    ccp.InstalmentDate);
        }
        
        public List<CreditCardPaymentDomainEntity> GetListByCreditCardExtre(CreditCardExtreDomainEntity creditCardExtre)
        {
            var fromDate = creditCardExtre.ExtreDate.AddMonths(-1);
            var transactions = coreContext.Query<IAccountTransactionRepository>()
                .GetLastOutgoingDateRangeListByTransactionOwner(creditCardExtre.CreditCard, fromDate, creditCardExtre.ExtreDate);

            return GetOrderedDescListBy(
                ccp =>
                    transactions.Select(t => t.TransactionId).Contains(ccp.AccountTransactionId),
                ccp =>
                    ccp.InstalmentDate);
        }
    }

    public interface ICreditCardPaymentRepository : IRepository<CreditCardPaymentDomainEntity>
    {
        List<CreditCardPaymentDomainEntity> GetListByCreditCard(CreditCardDomainEntity creditCard, DateTime fromDate);
        List<CreditCardPaymentDomainEntity> GetListByCreditCardExtre(CreditCardExtreDomainEntity creditCardExtre);
    }
}
