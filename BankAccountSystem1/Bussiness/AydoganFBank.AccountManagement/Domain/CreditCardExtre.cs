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
    public class CreditCardExtreDomainEntity : IDomainEntity, ICreditCardExtreInfo
    {
        private const decimal MIN_PAYMENT_RATIO = 0.25M;

        #region IoC
        private readonly ICoreContext coreContext;
        private readonly ICreditCardExtreRepository creditCardExtreRepository;

        public CreditCardExtreDomainEntity(
            ICoreContext coreContext,
            ICreditCardExtreRepository creditCardExtreRepository)
        {
            this.coreContext = coreContext;
            this.creditCardExtreRepository = creditCardExtreRepository;
        }
        #endregion

        public int CreditCardExtreId { get; set; }
        public CreditCardDomainEntity CreditCard { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal MinPayment { get; set; }
        public bool IsDischarged { get; set; }
        public bool IsMinDischarged { get; set; }

        int IDomainEntity.Id => CreditCardExtreId;

        #region Calculated properties
        public DateTime ExtreDate
        {
            get
            {
                return new DateTime(Year, Month, CreditCard.ExtreDay, 0, 0, 0);
            }
        }

        public DateTime ExtreStartDate => ExtreDate;
        public DateTime ExtreEndDate => ExtreDate.AddMonths(1);

        #endregion

        int ICreditCardExtreInfo.Id => CreditCardExtreId;
        int ICreditCardExtreInfo.Month => Month;
        string ICreditCardExtreInfo.MonthName => MonthName;
        int ICreditCardExtreInfo.Year => Year;
        decimal ICreditCardExtreInfo.TotalPayment => TotalPayment;
        decimal ICreditCardExtreInfo.MinPayment => MinPayment;
        bool ICreditCardExtreInfo.IsDischarged => IsDischarged;
        bool ICreditCardExtreInfo.IsMinDischarged => IsMinDischarged;
        DateTime ICreditCardExtreInfo.ExtreStartDate => ExtreStartDate;
        DateTime ICreditCardExtreInfo.ExtreEndDate => ExtreEndDate;
        DateTime ICreditCardExtreInfo.LastPaymentDate
        {
            get
            {
                if (Month == 12)
                    return new DateTime(Year + 1, 1, CreditCard.ExtreDay);
                else
                    return new DateTime(Year, Month + 1, CreditCard.ExtreDay);
            }
        }

        #region CRUD
        public void Insert()
        {
            creditCardExtreRepository.InsertEntity(this);
        }

        public void Save()
        {
            creditCardExtreRepository.UpdateEntity(this);
        }
        #endregion

        public CreditCardExtreDomainEntity With(
            CreditCardDomainEntity creditCard, int month, string monthName, int year, 
            decimal totalPayment, bool isDischarged, bool isMinDischarged)
        {
            CreditCard = creditCard ?? throw new CommonException.RequiredParameterMissingException(nameof(creditCard));
            Month = month;
            MonthName = monthName;
            Year = year;
            TotalPayment = totalPayment;
            MinPayment = totalPayment * MIN_PAYMENT_RATIO;
            IsDischarged = isDischarged;
            IsMinDischarged = isMinDischarged;

            return this;
        }

        public void AddPayment(decimal paymentAmount)
        {
            TotalPayment += paymentAmount;
            MinPayment = TotalPayment * MIN_PAYMENT_RATIO;

            Save();
        }

        public void Discharge()
        {
            IsDischarged = true;
            Save();
        }

        public void MinDischarge()
        {
            IsMinDischarged = true;
            Save();
        }

        public CreditCardExtreDischargeDomainEntity GetCreditCardExtreDischarge()
        {
            return coreContext.Query<ICreditCardExtreDischargeRepository>().GetByCreditCardExtre(this);
        }

        public List<CreditCardPaymentDomainEntity> GetPayments()
        {
            return coreContext.Query<ICreditCardPaymentRepository>().GetCreditCardPaymentsByDateRange(CreditCard, ExtreStartDate, ExtreEndDate);
        }
    }


    public class CreditCardExtreRepository : OrderedQueryRepository<CreditCardExtreDomainEntity, CreditCardExtre>, ICreditCardExtreRepository
    {
        public CreditCardExtreRepository(
            ICoreContext coreContext) 
            : base(coreContext)
        {
        }

        #region Mapping overrides
        public override void MapToDbEntity(CreditCardExtreDomainEntity domainEntity, CreditCardExtre dbEntity)
        {
            dbEntity.CreditCardId = domainEntity.CreditCard.CreditCardId;
            dbEntity.IsDischarged = domainEntity.IsDischarged;
            dbEntity.IsMinDischarged = domainEntity.IsMinDischarged;
            dbEntity.MinPayment = domainEntity.MinPayment;
            dbEntity.Month = domainEntity.Month;
            dbEntity.MonthName = domainEntity.MonthName;
            dbEntity.TotalPayment = domainEntity.TotalPayment;
            dbEntity.Year = domainEntity.Year;
        }

        public override void MapToDomainObject(CreditCardExtreDomainEntity domainEntity, CreditCardExtre dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.CreditCard = coreContext.Query<ICreditCardRepository>().GetById(dbEntity.CreditCardId);
            domainEntity.CreditCardExtreId = dbEntity.CreditCardExtreId;
            domainEntity.IsDischarged = dbEntity.IsDischarged;
            domainEntity.IsMinDischarged = dbEntity.IsMinDischarged;
            domainEntity.MinPayment = dbEntity.MinPayment;
            domainEntity.Month = dbEntity.Month;
            domainEntity.MonthName = dbEntity.MonthName;
            domainEntity.TotalPayment = dbEntity.TotalPayment;
            domainEntity.Year = dbEntity.Year;
        }
        #endregion

        public override CreditCardExtreDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override CreditCardExtre GetDbEntityById(int id)
        {
            return dbContext.CreditCardExtre.Single(cce => cce.CreditCardExtreId == id);
        }

        public CreditCardExtreDomainEntity LastBy(CreditCardDomainEntity creditCard)
        {
            return GetLastBy(
                cce =>
                    cce.CreditCardId == creditCard.CreditCardId);
        }

        public List<CreditCardExtreDomainEntity> By(CreditCardDomainEntity creditCard)
        {
            return GetOrderedDescListBy(
                cce =>
                    cce.CreditCardId == creditCard.CreditCardId,
                cce =>
                    cce.Year,
                cce =>
                    cce.Month);
        }

        public CreditCardExtreDomainEntity SingleBy(CreditCardDomainEntity creditCard, int month, int year)
        {
            return GetFirstBy(
                cce =>
                    cce.CreditCardId == creditCard.CreditCardId && cce.Month == month && cce.Year == year);
        }

        public List<CreditCardExtreDomainEntity> By(CreditCardDomainEntity creditCard, int month, int year)
        {
            return GetListBy(
                cce =>
                    cce.CreditCardId == creditCard.CreditCardId &&
                    (cce.Month >= month &&
                    cce.Year >= year) ||
                    cce.Year > year);
        }

        CreditCardExtreDomainEntity ICreditCardExtreRepository.GetByCreditCardAndDate(CreditCardDomainEntity creditCard, int month, int year)
            => SingleBy(creditCard, month, year);

        List<CreditCardExtreDomainEntity> ICreditCardExtreRepository.GetListByCreditCard(CreditCardDomainEntity creditCard)
            => By(creditCard);

        CreditCardExtreDomainEntity ICreditCardExtreRepository.GetLastByCreditCard(CreditCardDomainEntity creditCard)
            => LastBy(creditCard);

        List<CreditCardExtreDomainEntity> ICreditCardExtreRepository.GetActiveListByCreditCardExtre(CreditCardDomainEntity creditCard, int month, int year)
            => By(creditCard, month, year);
    }

    public interface ICreditCardExtreRepository : IRepository<CreditCardExtreDomainEntity>
    {
        CreditCardExtreDomainEntity GetLastByCreditCard(CreditCardDomainEntity creditCard);
        List<CreditCardExtreDomainEntity> GetListByCreditCard(CreditCardDomainEntity creditCard);
        CreditCardExtreDomainEntity GetByCreditCardAndDate(CreditCardDomainEntity creditCard, int month, int year);
        List<CreditCardExtreDomainEntity> GetActiveListByCreditCardExtre(CreditCardDomainEntity creditCard, int month, int year);
    }
}
