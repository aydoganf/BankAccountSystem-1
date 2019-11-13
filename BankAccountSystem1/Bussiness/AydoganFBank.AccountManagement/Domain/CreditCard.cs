using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context;
using AydoganFBank.Context.IoC;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using AydoganFBank.Context.Exception;
using System.Globalization;

namespace AydoganFBank.AccountManagement.Domain
{
    public class CreditCardDomainEntity : IDomainEntity, ITransactionOwner, ITransactionDetailOwner, ICreditCardInfo
    {
        #region IoC
        private readonly ICoreContext coreContext;
        private readonly ICreditCardRepository creditCardRepository;

        public CreditCardDomainEntity(
            ICoreContext coreContext,
            ICreditCardRepository creditCardRepository)
        {
            this.coreContext = coreContext;
            this.creditCardRepository = creditCardRepository;
        }
        #endregion

        public int CreditCardId { get; set; }
        public string CreditCardNumber { get; set; }
        public decimal Limit { get; set; }
        public int ExtreDay { get; set; }
        public decimal Debt { get; set; }
        public int Type { get; set; }
        public string ValidMonth { get; set; }
        public string ValidYear { get; set; }
        public string SecurityCode { get; set; }
        public bool IsInternetUsageOpen { get; set; }
        public ICreditCardOwner CreditCardOwner { get; set; }

        int IDomainEntity.Id => CreditCardId;

        int ITransactionOwner.OwnerId => CreditCardId;
        TransactionOwnerType ITransactionOwner.OwnerType => TransactionOwnerType.CreditCard;
        string ITransactionOwner.TransactionDetailDisplayName => string.Format("Credit card - {0}", CreditCardMaskedNumber);
        string ITransactionOwner.AssetsUnit => CreditCardOwner.AssetsUnit;

        int ITransactionDetailOwner.OwnerId => CreditCardId;
        TransactionDetailOwnerType ITransactionDetailOwner.OwnerType => TransactionDetailOwnerType.CreditCard;

        #region Calculated properties
        public string CreditCardMaskedNumber
        {
            get
            {
                return string.Format("**{0}", CreditCardNumber.Substring(CreditCardNumber.Length - 6));
            }
        }

        public decimal UsableLimit
        {
            get
            {
                return Limit - Debt;
            }
        }

        public DateTime UntilValidDate
        {
            get
            {
                int month = Convert.ToInt32(ValidMonth);
                int year = Convert.ToInt32(ValidYear);
                int day = 1;
                int hour = 0;
                int minute = 0;
                int second = 0;

                return new DateTime(year, month, day, hour, minute, second);
            }
        }
        #endregion

        #region Api
        int ICreditCardInfo.Id => CreditCardId;

        string ICreditCardInfo.CreditCardNumber => CreditCardNumber;

        decimal ICreditCardInfo.Limit => Limit;

        int ICreditCardInfo.ExtreDay => ExtreDay;

        decimal ICreditCardInfo.Debt => Debt;

        int ICreditCardInfo.Type => Type;

        string ICreditCardInfo.ValidMonth => ValidMonth;

        string ICreditCardInfo.ValidYear => ValidYear;

        string ICreditCardInfo.SecurityCode => SecurityCode;

        bool ICreditCardInfo.IsInternetUsageOpen => IsInternetUsageOpen;

        ICreditCardOwner ICreditCardInfo.CreditCardOwner => CreditCardOwner;

        string ICreditCardInfo.CreditCardMaskedNumber => CreditCardMaskedNumber;

        decimal ICreditCardInfo.UsableLimit => UsableLimit;

        DateTime ICreditCardInfo.UntilValidDate => UntilValidDate;
        #endregion

        public CreditCardDomainEntity With(
            decimal limit, int extreDay, int type, string validMonth, string validYear, string securityCode, 
            bool isInternetUsageOpen, ICreditCardOwner creditCardOwner)
        {
            if (limit <= 0)
                throw new AccountManagementException.CreditCardLimitCouldNotBeZeroOrNegative(string.Format("{0} = {1}", nameof(limit), limit));

            if (extreDay <= 0)
                throw new AccountManagementException.CreditCardExtreDayCouldNotZeroOrNegative(string.Format("{0} = {1}", nameof(extreDay), extreDay));

            CreditCardDomainEntity cc = null;

            try
            {
                cc = creditCardRepository.GetByCreditCardOwner(creditCardOwner);
            }
            catch (Exception ex)
            {
            }

            if (cc != null)
                throw new AccountManagementException.CreditCardOwnerHasAlreadyCreditCard(string.Format("Owner:{0} - Id: {1}", creditCardOwner.CreditCardOwnerType, creditCardOwner.OwnerId));

            CreditCardNumber = GenerateCrediCardNumber();
            Limit = limit;
            ExtreDay = extreDay;
            Debt = 0;
            Type = type;
            ValidMonth = string.IsNullOrWhiteSpace(validMonth) == false ? validMonth : throw new CommonException.RequiredParameterMissingException(nameof(validMonth));
            ValidYear = string.IsNullOrWhiteSpace(validYear) == false ? validYear : throw new CommonException.RequiredParameterMissingException(nameof(validYear));
            SecurityCode = string.IsNullOrWhiteSpace(securityCode) == false ? securityCode : throw new CommonException.RequiredParameterMissingException(nameof(securityCode));
            IsInternetUsageOpen = isInternetUsageOpen;
            CreditCardOwner = creditCardOwner ?? throw new CommonException.RequiredParameterMissingException(nameof(creditCardOwner)); ;
            return this;
        }

        private string GenerateCrediCardNumber()
        {
            Random rnd = new Random();
            const string numbers = "123456789";
            var stringChars = new char[16];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = numbers[rnd.Next(numbers.Length)];
            }

            var creditCardNumber = new String(stringChars);

            try
            {
                var cc = creditCardRepository.GetByCreditCardNumber(creditCardNumber);
                if (cc != null)
                {
                    creditCardNumber = GenerateCrediCardNumber();
                }
            }
            catch (Exception)
            {
            }

            return creditCardNumber;
        }

        public void Insert()
        {
            creditCardRepository.InsertEntity(this);
        }

        public void Save()
        {
            creditCardRepository.UpdateEntity(this);
        }

        public void Flush()
        {
            creditCardRepository.FlushEntity(this);
        }

        public void Delete()
        {
            creditCardRepository.DeleteEntity(this);
        }

        public void DoPayment(decimal amount)
        {
            if (UntilValidDate < DateTime.Now)
                throw new AccountManagementException.CreditCardValidDateHasExpired(string.Format("Valid date limit = {0}", UntilValidDate));

            if (UsableLimit < amount)
                throw new AccountManagementException.CreditCardHasNotEnoughLimit(string.Format("{0} = {1}", nameof(amount), amount));

            Debt += amount;
        }

        public void GetDischarge(decimal amount)
        {
            Debt -= amount;
            Flush();
        }

        public CreditCardExtreDomainEntity GetCurrentExtre()
        {
            DateTime now = DateTime.Now;

            int month = now.Month;
            int year = now.Year;

            // same month but earlier than extre day
            if (now.Day < ExtreDay)
            { 
                month -= 1;
            }

            // for january
            if (now.Day < ExtreDay && now.Month == 1)
            {
                year -= 1;
            }

            var extre = coreContext.Query<ICreditCardExtreRepository>().GetByCreditCardAndDate(this, month, year);

            if (extre == null)
            {
                extre = CreateExtre(now.Month, now.Year, 0);
            }

            return extre;
        }

        [Obsolete]
        public CreditCardExtreDomainEntity GetLastCreditCardExtre()
        {
            DateTime now = DateTime.Now;

            var extre = coreContext.Query<ICreditCardExtreRepository>().GetByCreditCardAndDate(this, now.Month, now.Year);

            if (extre == null)
            {
                extre = CreateExtre(now.Month, now.Year, 0);
            }

            return extre;
        }

        public CreditCardExtreDomainEntity CreateExtre(int month, int year, decimal totalPayment)
        {
            DateTime extreMonth = new DateTime(year, month, 1);
            var extre = coreContext.New<CreditCardExtreDomainEntity>().With(
                this,
                month,
                extreMonth.ToString("MMMM", CultureInfo.InvariantCulture),
                year,
                totalPayment,
                isDischarged: false,
                isMinDischarged: false);

            extre.Insert();
            return extre;
        }

        public List<CreditCardExtreDomainEntity> GetCreditCardExtres()
        {
            return coreContext.Query<ICreditCardExtreRepository>().GetListByCreditCard(this);
        }

        public CreditCardExtreDomainEntity GetCreditCardExtreByDate(int month, int year)
        {
            return coreContext.Query<ICreditCardExtreRepository>().GetByCreditCardAndDate(this, month, year);
        }

        public List<CreditCardExtreDischargeDomainEntity> GetExtreDischargeList(DateTime startDate, DateTime endDate)
        {
            return coreContext.Query<ICreditCardExtreDischargeRepository>().GetListByCreditCardAndDateRange(this, startDate, endDate);
        }

        public List<CreditCardExtreDomainEntity> GetCreditCardExtreListByDateRange(DateTime startDate, DateTime endDate)
        {
            return coreContext.Query<ICreditCardExtreRepository>().GetByCreditCardAndDateRange(this, startDate, endDate);
        }

        public List<AccountTransactionDomainEntity> GetLastDateRangeCreditCardTransactionList(DateTime startDate, DateTime endDate)
        {
            return coreContext.Query<IAccountTransactionRepository>()
                .GetLastDateRangeListByTransactionOwner(this, startDate, endDate);
        }

        public List<CreditCardPaymentDomainEntity> GetLastExtrePayments()
        {
            return GetCurrentExtre().GetPayments();
        }

        public List<TransactionDetailDomainEntity> GetLastTransactionDetailDateRangeList(DateTime startDate, DateTime endDate)
        {
            return coreContext.Query<ITransactionDetailRepository>()
                .GetLastDateRangeListByTransactionDetailOwner(this, startDate, endDate);
        }

        public List<TransactionDetailDomainEntity> GetTransactionDetailDateRangeList(DateTime startDate, DateTime endDate)
        {
            return coreContext.Query<ITransactionDetailRepository>()
                .GetDateRangeListByTransactionDetailOwner(this, startDate, endDate);
        }

        public List<CreditCardExtreDomainEntity> GetActiveExtreList()
        {
            DateTime now = DateTime.Now;
            return coreContext.Query<ICreditCardExtreRepository>().GetActiveListByCreditCardExtre(this, now.Month, now.Year);
        }

        public List<CreditCardPaymentDomainEntity> GetActivePaymentList()
        {
            var extre = GetCurrentExtre();
            return coreContext.Query<ICreditCardPaymentRepository>().GetCreditCardPaymentsByDateRange(this, extre.ExtreStartDate, DateTime.MaxValue);
        }
    }

    public class CreditCardRepository : Repository<CreditCardDomainEntity, CreditCard>, ICreditCardRepository
    {
        public CreditCardRepository(ICoreContext coreContext) 
            : base(coreContext)
        {
        }

        public override CreditCardDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override CreditCard GetDbEntityById(int id)
        {
            return dbContext.CreditCard.FirstOrDefault(cc => cc.CreditCardId == id);
        }

        private ICreditCardOwner GetCreditCardOwner(int ownerType, int ownerId)
        {
            ICreditCardOwner creditCardOwner = null;
            if (ownerType == CreditCardOwnerType.Account.ToInt())
            {
                creditCardOwner = coreContext.Query<IAccountRepository>()
                    .GetById(ownerId);
            }
            //else if (ownerType == CreditCardOwnerType.Company.ToInt())
            //{
            //    creditCardOwner = coreContext.Query<ICompanyRepository>()
            //        .GetById(ownerId);
            //}

            return creditCardOwner;
        }

        #region Mapping overrides
        public override void MapToDbEntity(CreditCardDomainEntity domainEntity, CreditCard dbEntity)
        {
            dbEntity.CreditCardNumber = domainEntity.CreditCardNumber;
            dbEntity.ExtreDay = domainEntity.ExtreDay;
            dbEntity.IsInternetUsageOpen = domainEntity.IsInternetUsageOpen;
            dbEntity.Limit = domainEntity.Limit;
            dbEntity.Debt = domainEntity.Debt;
            dbEntity.OwnerId = domainEntity.CreditCardOwner.OwnerId;
            dbEntity.OwnerType = (int)domainEntity.CreditCardOwner.CreditCardOwnerType;
            dbEntity.SecurityCode = domainEntity.SecurityCode;
            dbEntity.Type = domainEntity.Type;
            dbEntity.ValidMonth = domainEntity.ValidMonth;
            dbEntity.ValidYear = domainEntity.ValidYear;
        }

        public override void MapToDomainObject(CreditCardDomainEntity domainEntity, CreditCard dbEntity)
        {
            if (domainEntity == null || dbEntity == null)
                return;

            domainEntity.CreditCardId = dbEntity.CreditCardId;
            domainEntity.CreditCardNumber = dbEntity.CreditCardNumber;
            domainEntity.CreditCardOwner = GetCreditCardOwner(dbEntity.OwnerType, dbEntity.OwnerId); 
            domainEntity.ExtreDay = dbEntity.ExtreDay;
            domainEntity.IsInternetUsageOpen = dbEntity.IsInternetUsageOpen;
            domainEntity.Limit = dbEntity.Limit;
            domainEntity.Debt = dbEntity.Debt;
            domainEntity.SecurityCode = dbEntity.SecurityCode;
            domainEntity.Type = dbEntity.Type;
            domainEntity.ValidMonth = dbEntity.ValidMonth;
            domainEntity.ValidYear = dbEntity.ValidYear;
        }
        #endregion

        public CreditCardDomainEntity By(ICreditCardOwner creditCardOwner)
        {
            int ownerType = creditCardOwner.CreditCardOwnerType.ToInt();
            return GetFirstBy(
                cc => 
                    cc.OwnerType == ownerType && 
                    cc.OwnerId == creditCardOwner.OwnerId);
        }

        public CreditCardDomainEntity GetBySecurityInfos(
            string creditCardNumber, string validMonth, string validYear, string securityCode)
        {
            return GetFirstBy(
                cc =>
                    cc.CreditCardNumber == creditCardNumber && cc.ValidMonth == validMonth &&
                    cc.ValidYear == validYear && cc.SecurityCode == securityCode);
        }

        public CreditCardDomainEntity GetByCreditCardNumber(string creditCardNumber)
        {
            return GetFirstBy(
                cc =>
                    cc.CreditCardNumber == creditCardNumber);
        }

        CreditCardDomainEntity ICreditCardRepository.GetByCreditCardOwner(ICreditCardOwner creditCardOwner) => By(creditCardOwner);
    }

    public interface ICreditCardRepository : IRepository<CreditCardDomainEntity>
    {
        CreditCardDomainEntity GetByCreditCardNumber(string creditCardNumber);
        CreditCardDomainEntity GetByCreditCardOwner(ICreditCardOwner creditCardOwner);

        CreditCardDomainEntity GetBySecurityInfos(string creditCardNumber, string validMonth, string validYear, string securityCode);
    }
}
