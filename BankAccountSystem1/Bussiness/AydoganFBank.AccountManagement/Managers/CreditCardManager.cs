using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Context.IoC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AydoganFBank.AccountManagement.Managers
{
    public class CreditCardManager : ICreditCardManager
    {
        #region IoC
        private readonly ICoreContext coreContext;
        private readonly AccountManager accountManager;

        public CreditCardManager(
            ICoreContext coreContext, 
            AccountManager accountManager)
        {
            this.coreContext = coreContext;
            this.accountManager = accountManager;
        }
        #endregion

        internal CreditCardDomainEntity GetCreditCardByAccount(AccountDomainEntity account)
        {
            return coreContext.Query<ICreditCardRepository>().GetByCreditCardOwner(account);
        }

        internal CreditCardDomainEntity GetCreditCardByAccount(int accountId)
        {
            return GetCreditCardByAccount(accountManager.GetAccountById(accountId));
        }

        internal CreditCardDomainEntity GetCreditCardByAccount(string accountNumber)
        {
            var account = accountManager.GetAccountByAccountNumber(accountNumber);
            return GetCreditCardByAccount(account);
        }

        internal List<CreditCardDomainEntity> GetCreditCardListByPerson(PersonDomainEntity person)
        {
            var accounts = accountManager.GetAccountsByPerson(person);
            List<CreditCardDomainEntity> creditCards = new List<CreditCardDomainEntity>();
            foreach (var account in accounts)
            {
                var creditCard = coreContext.Query<ICreditCardRepository>().GetByCreditCardOwner(account);
                if(creditCard != null)
                    creditCards.Add(creditCard);
            }

            return creditCards;
        }

        internal List<CreditCardDomainEntity> GetCreditCardListByPerson(int personId)
        {
            var person = coreContext.Query<IPersonRepository>().GetById(personId);
            return GetCreditCardListByPerson(person);
        }

        internal CreditCardDomainEntity GetCreditCardById(int creditCardId)
        {
            return coreContext.Query<ICreditCardRepository>()
                .GetById(creditCardId);
        }

        internal CreditCardDomainEntity GetCreditCardBySecurityInfos(
            string creditCardNumber, string validMonth, string validYear, string securityCode)
        {
            return coreContext.Query<ICreditCardRepository>().GetBySecurityInfos(creditCardNumber, validMonth, validYear, securityCode);
        }

        private CreditCardDomainEntity CreateCreditCard(decimal limit, int extreDay, int type, string validMonth, string validYear, string securityCode,
            bool isInternetUsageOpen, ICreditCardOwner creditCardOwner)
        {
            var creditCard = coreContext.New<CreditCardDomainEntity>()
                .With(limit, extreDay, type, validMonth, validYear, securityCode, isInternetUsageOpen, creditCardOwner);
            creditCard.Insert();

            return creditCard;
        }

        internal CreditCardDomainEntity CreateAccountCreditCard(decimal limit, int extreDay, int type, string validMonth, string validYear, string securityCode, bool isInternetUsageOpen, int accountId)
        {
            var account = coreContext.Query<IAccountRepository>()
                .GetById(accountId);

            return CreateCreditCard(limit, extreDay, type, validMonth, validYear, securityCode, isInternetUsageOpen, account);
        }

        private CreditCardDomainEntity DoCreditCardPayment(CreditCardDomainEntity creditCard, decimal amount, int instalmentCount, ITransactionOwner toTransactionOwner)
        {
            creditCard.DoPayment(amount);

            var transaction = coreContext.New<AccountTransactionDomainEntity>()
                .With(
                    creditCard, 
                    toTransactionOwner, 
                    amount, 
                    TransactionTypeEnum.CreditCardPayment, 
                    TransactionStatusEnum.InProgress, 
                    creditCard);

            transaction.Insert();

            var transactionDetailIn = transaction.CreateTransactionDetail(TransactionDirection.In);
            transactionDetailIn.Insert(forceToInsertDb: false);
            var transactionDetailOut = transaction.CreateTransactionDetail(TransactionDirection.Out);
            transactionDetailOut.Insert(forceToInsertDb: false);
            
            decimal instalmentAmount = amount / instalmentCount;

            for (int instalmentIndex = 1; instalmentIndex <= instalmentCount; instalmentIndex++)
            {
                string paymentDescription = string.Format("{0} - {1} {2} ({3} instalment)",
                    toTransactionOwner.TransactionDetailDisplayName, 
                    instalmentAmount, 
                    toTransactionOwner.AssetsUnit,
                    string.Format("{0}/{1}", instalmentIndex, instalmentCount));

                DateTime instalmentDate = transaction.TransactionDate.AddMonths(instalmentIndex - 1);

                var creditCardPayment = coreContext.New<CreditCardPaymentDomainEntity>()
                    .With(
                        instalmentIndex, 
                        instalmentAmount, 
                        paymentDescription,
                        transaction.TransactionDate, 
                        instalmentDate, 
                        transaction,
                        creditCard);
                creditCardPayment.Insert(forceToInsertDb: false);

                var extre = coreContext.Query<ICreditCardExtreRepository>().GetByCreditCardAndDate(creditCard, instalmentDate.Month, instalmentDate.Year);
                if (extre == null)
                {
                    extre = creditCard.CreateExtre(instalmentDate.Month, instalmentDate.Year, instalmentAmount);
                }
            }

            creditCard.Flush();

            transaction.SetStatus(TransactionStatusEnum.Succeeded);

            coreContext.Commit();

            return creditCard;
        }

        internal CreditCardDomainEntity DoCreditCardPayment(int creditCardId, decimal amount, int instalmentCount, ITransactionOwner toTransactionOwner)
        {
            var creditCard = GetCreditCardById(creditCardId);

            return DoCreditCardPayment(creditCard, amount, instalmentCount, toTransactionOwner);
        }

        internal CreditCardDomainEntity DoCreditCardPayment(int creditCardId, decimal amount, int instalmentCount, int toAccountId)
        {
            var creditCard = GetCreditCardById(creditCardId);

            var toAccount = accountManager.GetAccountById(toAccountId);

            return DoCreditCardPayment(creditCard, amount, instalmentCount, toAccount);
        }

        internal CreditCardDomainEntity DoCreditCardPayment(
            string creditCardNumber, string validMonth, string validYear, string securityCode, 
            decimal amount, int instalmentCount, int toAccountId)
        {
            var creditCard = GetCreditCardBySecurityInfos(creditCardNumber, validMonth, validYear, securityCode);
            var toAccount = accountManager.GetAccountById(toAccountId);

            return DoCreditCardPayment(creditCard, amount, instalmentCount, toAccount);
        }

        internal List<CreditCardPaymentDomainEntity> GetLastExtrePaymentsByCreditCardId(int creditCardId)
        {
            var creditCard = GetCreditCardById(creditCardId);
            return creditCard.GetLastExtrePayments();
        }

        internal List<CreditCardExtreDomainEntity> GetCreditCardActiveExtreList(int creditCardId)
        {
            var creditCard = GetCreditCardById(creditCardId);
            return creditCard.GetActiveExtreList();
        }

        internal CreditCardExtreDomainEntity GetExtreById(int extreId)
        {
            return coreContext.Query<ICreditCardExtreRepository>().GetById(extreId);
        }

        internal List<CreditCardPaymentDomainEntity> GetExtrePaymentList(int extreId)
        {
            return GetExtreById(extreId).GetPayments();
        }
        #region API Implementations

        ICreditCardInfo ICreditCardManager.GetCreditCardByAccount(string accountNumber) => GetCreditCardByAccount(accountNumber);

        List<ICreditCardInfo> ICreditCardManager.GetCreditCardListByPerson(int personId) => GetCreditCardListByPerson(personId).Cast<ICreditCardInfo>().ToList();

        ICreditCardInfo ICreditCardManager.CreateCreditCard(
            decimal limit, 
            int extreDate, 
            int type, 
            string validMonth, 
            string validYear, 
            string securityCode, 
            bool isInternetUsageOpen, 
            ICreditCardOwner creditCardOwner) 
                => CreateCreditCard(limit, extreDate, type, validMonth, validYear, securityCode, isInternetUsageOpen, creditCardOwner);

        ICreditCardInfo ICreditCardManager.CreateAccountCreditCard(
            decimal limit, 
            int extreDay, 
            int type, 
            string validMonth, 
            string validYear, 
            string securityCode, 
            bool isInternetUsageOpen, 
            int accountId) 
                => CreateAccountCreditCard(limit, extreDay, type, validMonth, validYear, securityCode, isInternetUsageOpen, accountId);

        ICreditCardInfo ICreditCardManager.DoCreditCardPayment(
            int creditCardId, 
            decimal amount, 
            int instalmentCount, 
            ITransactionOwner toTransactionOwner) 
                => DoCreditCardPayment(creditCardId, amount, instalmentCount, toTransactionOwner);

        ICreditCardInfo ICreditCardManager.DoCreditCardPayment(
            int creditCardId, 
            decimal amount, 
            int instalmentCount, 
            int toAccountId) 
                => DoCreditCardPayment(creditCardId, amount, instalmentCount, toAccountId);

        ICreditCardInfo ICreditCardManager.DoCreditCardPayment(
            string creditCardNumber, 
            string validMonth, 
            string validYear, 
            string securityCode, 
            decimal amount, 
            int instalmentCount, 
            int toAccountId) 
                => DoCreditCardPayment(creditCardNumber, validMonth, validYear, securityCode, amount, instalmentCount, toAccountId);

        ICreditCardInfo ICreditCardManager.GetCreditCardById(int creditCardId) => GetCreditCardById(creditCardId);

        List<ICreditCardPaymentInfo> ICreditCardManager.GetCreditCardLastExtrePayments(int creditCardId)
            => GetLastExtrePaymentsByCreditCardId(creditCardId).Cast<ICreditCardPaymentInfo>().ToList();

        List<ICreditCardExtreInfo> ICreditCardManager.GetCreditCardActiveExtreList(int creditCardId)
            => GetCreditCardActiveExtreList(creditCardId).Cast<ICreditCardExtreInfo>().ToList();

        List<ICreditCardPaymentInfo> ICreditCardManager.GetExtrePaymentList(int extreId)
            => GetExtrePaymentList(extreId).Cast<ICreditCardPaymentInfo>().ToList();
        #endregion
    }
}
