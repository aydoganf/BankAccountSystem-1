using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Context;
using AydoganFBank.Context.IoC;
using AydoganFBank.Context.DataAccess;
using AydoganFBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using AydoganFBank.Context.Exception;

namespace AydoganFBank.AccountManagement.Domain
{
    public class CreditCardDomainEntity : IDomainEntity, ITransactionOwner
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

        #region Calculated properties
        public string CreditCardMaskedNumber
        {
            get
            {
                return string.Format("**{0}", CreditCardNumber.Substring(-6));
            }
        }

        public decimal UsableLimit
        {
            get
            {
                return Limit - Debt;
            }
        }
        #endregion

        public CreditCardDomainEntity With(
            decimal limit, int extreDay, int type, string validMonth, string validYear, string securityCode, 
            bool isInternetUsageOpen, ICreditCardOwner creditCardOwner)
        {
            if (limit <= 0)
                throw new AccountManagementException.CreditCardLimitCouldNotBeZeroOrNegative(string.Format("{0} = {1}", nameof(limit), limit));

            if (extreDay <= 0)
                throw new AccountManagementException.CreditCardExtreDayCouldNotZeroOrNegative(string.Format("{0} = {1}", nameof(extreDay), extreDay));

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
            return "";
        }

        public void Insert()
        {
            creditCardRepository.InsertEntity(this);
        }

        public void Save()
        {
            creditCardRepository.UpdateEntity(this);
        }

        public void DoPayment(decimal amount)
        {
            if (UsableLimit < amount)
                throw new AccountManagementException.CreditCardHasNotEnoughLimit(string.Format("{0} = {1}", nameof(amount), amount));

            Debt += amount;
        }

        public CreditCardExtreDomainEntity GetLastCreditCardExtre()
        {
            return coreContext.Query<ICreditCardExtreRepository>().GetLastByCreditCard(this);
        }

        public List<CreditCardExtreDomainEntity> GetCreditCardExtres()
        {
            return coreContext.Query<ICreditCardExtreRepository>().GetListByCreditCard(this);
        }

        public CreditCardExtreDomainEntity GetCreditCardExtreByDate(int month, int year)
        {
            return coreContext.Query<ICreditCardExtreRepository>().GetByCreditCardAndDate(this, month, year);
        }

        public List<AccountTransactionDomainEntity> GetTransactionInfoListByDateRange(DateTime startDate, DateTime endDate)
        {
            return coreContext.Query<IAccountTransactionRepository>()
                .GetLastOutgoingDateRangeListByTransactionOwner(this, startDate, endDate);
        }

        public List<CreditCardPaymentDomainEntity> GetLastExtrePayments()
        {
            var lastExtre = GetLastCreditCardExtre();
            return coreContext.Query<ICreditCardPaymentRepository>().GetListByCreditCardExtre(lastExtre);
        }
    }

    public class CreditCardRepository : 
        Repository<CreditCardDomainEntity, CreditCard>,
        IDomainObjectBuilderRepository<CreditCardDomainEntity, CreditCard>,
        ICreditCardRepository
    {
        public CreditCardRepository(ICoreContext coreContext) 
            : base(coreContext, null, null)
        {
        }

        public override CreditCardDomainEntity GetById(int id)
        {
            return MapToDomainObject(GetDbEntityById(id));
        }

        protected override CreditCard GetDbEntityById(int id)
        {
            return dbContext.CreditCard.Single(cc => cc.CreditCardId == id);
        }

        private ICreditCardOwner GetCreditCardOwner(int ownerType, int ownerId)
        {
            ICreditCardOwner creditCardOwner = null;
            if (ownerType == CreditCardOwnerType.Account.ToInt())
            {
                creditCardOwner = coreContext.Query<IAccountRepository>()
                    .GetById(ownerId);
            }
            else if (ownerType == CreditCardOwnerType.Company.ToInt())
            {
                creditCardOwner = coreContext.Query<ICompanyRepository>()
                    .GetById(ownerId);
            }

            return creditCardOwner;
        }

        #region Mapping overrides
        public override void MapToDbEntity(CreditCardDomainEntity domainEntity, CreditCard dbEntity)
        {
            dbEntity.CreditCardNumber = domainEntity.CreditCardNumber;
            dbEntity.ExtreDay = domainEntity.ExtreDay;
            dbEntity.IsInternetUsageOpen = domainEntity.IsInternetUsageOpen;
            dbEntity.Limit = domainEntity.Limit;
            dbEntity.OwnerId = domainEntity.CreditCardOwner.OwnerId;
            dbEntity.OwnerType = (int)domainEntity.CreditCardOwner.CreditCardOwnerType;
            dbEntity.SecurityCode = domainEntity.SecurityCode;
            dbEntity.Type = domainEntity.Type;
            dbEntity.ValidMonth = domainEntity.ValidMonth;
            dbEntity.ValidYear = domainEntity.ValidYear;
        }

        public override CreditCardDomainEntity MapToDomainObject(CreditCard dbEntity)
        {
            var domainEntity = coreContext.New<CreditCardDomainEntity>();
            MapToDomainObject(domainEntity, dbEntity);
            return domainEntity;
        }

        public override void MapToDomainObject(CreditCardDomainEntity domainEntity, CreditCard dbEntity)
        {
            domainEntity.CreditCardId = dbEntity.CreditCardId;
            domainEntity.CreditCardNumber = dbEntity.CreditCardNumber;
            domainEntity.CreditCardOwner = GetCreditCardOwner(dbEntity.OwnerType, dbEntity.OwnerId); 
            domainEntity.ExtreDay = dbEntity.ExtreDay;
            domainEntity.IsInternetUsageOpen = dbEntity.IsInternetUsageOpen;
            domainEntity.Limit = dbEntity.Limit;
            domainEntity.SecurityCode = dbEntity.SecurityCode;
            domainEntity.Type = dbEntity.Type;
            domainEntity.ValidMonth = dbEntity.ValidMonth;
            domainEntity.ValidYear = dbEntity.ValidYear;
        }

        public override IEnumerable<CreditCardDomainEntity> MapToDomainObjectList(IEnumerable<CreditCard> dbEntities)
        {
            List<CreditCardDomainEntity> domainEntities = new List<CreditCardDomainEntity>();
            foreach (var dbEntity in dbEntities)
            {
                domainEntities.Add(MapToDomainObject(dbEntity));
            }
            return domainEntities;
        }

        #endregion

        public CreditCardDomainEntity GetByCreditCardOwner(ICreditCardOwner creditCardOwner)
        {
            return GetFirstBy(
                cc => cc.OwnerType == creditCardOwner.CreditCardOwnerType.ToInt() && 
                cc.OwnerId == creditCardOwner.OwnerId);
        }

        
    }

    public interface ICreditCardRepository : IRepository<CreditCardDomainEntity>
    {
        CreditCardDomainEntity GetByCreditCardOwner(ICreditCardOwner creditCardOwner);
    }
}
