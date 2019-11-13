using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context;
using AydoganFBank.Context.Builders;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Context.Exception;
using AydoganFBank.Context.IoC;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AydoganFBank.AccountManagement.Domain
{
    public class CreditCardExtreDischargeDomainEntity : IDomainEntity, ITransactionHolder, ITransactionDetailOwner
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
        public CreditCardDomainEntity CreditCard { get; set; }


        int IDomainEntity.Id => CreditCardExtreDischargeId;

        ITransactionInfo ITransactionHolder.TransactionInfo => AccountTransaction;
        DateTime ITransactionHolder.CreateDate => CreateDate;

        int ITransactionDetailOwner.OwnerId => CreditCardExtreDischargeId;
        TransactionDetailOwnerType ITransactionDetailOwner.OwnerType => TransactionDetailOwnerType.CreditCardExtreDischarge;

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
            decimal dischargeAmount, 
            DateTime createDate, 
            CreditCardExtreDomainEntity creditCardExtre, 
            AccountTransactionDomainEntity accountTransaction,
            CreditCardDomainEntity creditCard)
        {
            DischargeAmount = dischargeAmount;
            CreateDate = createDate;
            CreditCardExtre = creditCardExtre ?? throw new CommonException.RequiredParameterMissingException(nameof(creditCardExtre));
            AccountTransaction = accountTransaction ?? throw new CommonException.RequiredParameterMissingException(nameof(accountTransaction));
            CreditCard = creditCard ?? throw new CommonException.RequiredParameterMissingException(nameof(creditCard));

            return this;
        }

        public TransactionDetailDomainEntity GenerateTransactionDetail(IAccountInfo fromAccount)
        {
            return coreContext.New<TransactionDetailDomainEntity>().With(
                $"Discharge operation from {fromAccount.AccountNumber}", DateTime.Now, DischargeAmount, AccountTransaction, this, TransactionDirection.In);
        }
    }

    public class CreditCardExtreDischargeRepository : OrderedQueryRepository<CreditCardExtreDischargeDomainEntity, CreditCardExtreDischarge>,
        ICreditCardExtreDischargeRepository
    {
        public CreditCardExtreDischargeRepository(
            ICoreContext coreContext) 
            : base(coreContext)
        {
        }

        #region Mapping overrides
        public override void MapToDbEntity(CreditCardExtreDischargeDomainEntity domainEntity, CreditCardExtreDischarge dbEntity)
        {
            dbEntity.AccountTransactionId = domainEntity.AccountTransaction.TransactionId;
            dbEntity.CreateDate = domainEntity.CreateDate;
            dbEntity.CreditCardExtreId = domainEntity.CreditCardExtre.CreditCardExtreId;
            dbEntity.DischargeAmount = domainEntity.DischargeAmount;
            dbEntity.CreditCardId = domainEntity.CreditCard.CreditCardId;
        }

        public override void MapToDomainObject(CreditCardExtreDischargeDomainEntity domainEntity, CreditCardExtreDischarge dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.AccountTransaction = coreContext.Query<IAccountTransactionRepository>().GetById(dbEntity.AccountTransactionId);
            domainEntity.CreateDate = dbEntity.CreateDate;
            domainEntity.CreditCardExtre = coreContext.Query<ICreditCardExtreRepository>().GetById(dbEntity.CreditCardExtreId);
            domainEntity.CreditCardExtreDischargeId = dbEntity.CreditCardExtreDischargeId;
            domainEntity.DischargeAmount = dbEntity.DischargeAmount;
            domainEntity.CreditCard = coreContext.Query<ICreditCardRepository>().GetById(dbEntity.CreditCardId);
        }
        #endregion

        public override CreditCardExtreDischargeDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override CreditCardExtreDischarge GetDbEntityById(int id)
        {
            return dbContext.CreditCardExtreDischarge.Single(cced => cced.CreditCardExtreDischargeId == id);
        }

        public List<CreditCardExtreDischargeDomainEntity> By(CreditCardExtreDomainEntity creditCardExtre)
        {
            return GetListBy(
                cced =>
                    cced.CreditCardExtreId == creditCardExtre.CreditCardExtreId);
        }

        public List<CreditCardExtreDischargeDomainEntity> By(CreditCardDomainEntity creditCard, DateTime startDate, DateTime endDate)
        {
            return GetListBy(
                cced =>
                    cced.CreditCardId == creditCard.CreditCardId &&
                    cced.CreateDate >= startDate &&
                    cced.CreateDate <= endDate);
        }

        List<CreditCardExtreDischargeDomainEntity> ICreditCardExtreDischargeRepository.GetByCreditCardExtre(CreditCardExtreDomainEntity creditCardExtre)
            => By(creditCardExtre);

        List<CreditCardExtreDischargeDomainEntity> ICreditCardExtreDischargeRepository.GetListByCreditCardAndDateRange(CreditCardDomainEntity creditCard, DateTime startDate, DateTime endDate)
            => By(creditCard, startDate, endDate);
    }

    public interface ICreditCardExtreDischargeRepository : IRepository<CreditCardExtreDischargeDomainEntity>
    {
        List<CreditCardExtreDischargeDomainEntity> GetByCreditCardExtre(CreditCardExtreDomainEntity creditCardExtre);
        List<CreditCardExtreDischargeDomainEntity> GetListByCreditCardAndDateRange(CreditCardDomainEntity creditCard, DateTime startDate, DateTime endDate);
    }
}
