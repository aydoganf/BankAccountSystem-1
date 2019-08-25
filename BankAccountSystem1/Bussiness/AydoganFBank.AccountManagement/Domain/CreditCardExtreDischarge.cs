using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Context.Exception;
using AydoganFBank.Context.IoC;
using AydoganFBank.Database;
using System;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
{
    public class CreditCardExtreDischargeDomainEntity : IDomainEntity, ITransactionHolder
    {
        #region IoC
        private readonly ICoreContext coreContext;
        private readonly ICreditCardExtreDischargeRepository creditCardExtreDischargeRepository;

        public CreditCardExtreDischargeDomainEntity(
            ICoreContext coreContext,
            ICreditCardExtreDischargeRepository creditCardExtreDischargeRepository)
        {
            this.coreContext = coreContext;
            this.creditCardExtreDischargeRepository = creditCardExtreDischargeRepository;
        }
        #endregion

        public int CreditCardExtreDischargeId { get; set; }
        public decimal DischargeAmount { get; set; }
        public DateTime CreateDate { get; set; }
        public CreditCardExtreDomainEntity CreditCardExtre { get; set; }
        public AccountTransactionDomainEntity AccountTransaction { get; set; }


        int IDomainEntity.Id => CreditCardExtreDischargeId;

        ITransactionInfo ITransactionHolder.TransactionInfo => AccountTransaction;
        DateTime ITransactionHolder.CreateDate => CreateDate;

        public CreditCardDomainEntity CreditCard => CreditCardExtre.CreditCard;

        #region CRUD
        public void Insert()
        {
            creditCardExtreDischargeRepository.InsertEntity(this);
        }

        public void Save()
        {
            creditCardExtreDischargeRepository.UpdateEntity(this);
        }
        #endregion

        public CreditCardExtreDischargeDomainEntity With(
            decimal dischargeAmount, DateTime createDate, 
            CreditCardExtreDomainEntity creditCardExtre, AccountTransactionDomainEntity accountTransaction)
        {
            DischargeAmount = dischargeAmount;
            CreateDate = createDate;
            CreditCardExtre = creditCardExtre ?? throw new CommonException.RequiredParameterMissingException(nameof(creditCardExtre));
            AccountTransaction = accountTransaction ?? throw new CommonException.RequiredParameterMissingException(nameof(accountTransaction));

            return this;
        }
    }

    public class CreditCardExtreDischargeRepository :
        OrderedQueryRepository<CreditCardExtreDischargeDomainEntity, CreditCardExtreDischarge>
    {
        public CreditCardExtreDischargeRepository(
            ICoreContext coreContext, 
            IDomainEntityBuilder<CreditCardExtreDischargeDomainEntity, CreditCardExtreDischarge> domainEntityBuilder, 
            IDbEntityMapper<CreditCardExtreDischarge, CreditCardExtreDischargeDomainEntity> dbEntityMapper) 
            : base(coreContext, domainEntityBuilder, dbEntityMapper)
        {
        }

        public override CreditCardExtreDischargeDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override CreditCardExtreDischarge GetDbEntityById(int id)
        {
            return dbContext.CreditCardExtreDischarge.Single(cced => cced.CreditCardExtreDischargeId == id);
        }

        public CreditCardExtreDischargeDomainEntity GetByCreditCardExtre(CreditCardExtreDomainEntity creditCardExtre)
        {
            return GetFirstBy(
                cced =>
                    cced.CreditCardExtreId == creditCardExtre.CreditCardExtreId);
        }
    }

    public interface ICreditCardExtreDischargeRepository : IRepository<CreditCardExtreDischargeDomainEntity>
    {
        CreditCardExtreDischargeDomainEntity GetByCreditCardExtre(CreditCardExtreDomainEntity creditCardExtre);
    }
}
