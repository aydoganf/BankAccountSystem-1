using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Context.IoC;
using System;

namespace AydoganFBank.AccountManagement.Managers
{
    public class CreditCardManager : ICreditCardManager
    {
        #region IoC
        private readonly ICoreContext coreContext;
        private readonly AccountManager accountManager;
        private readonly CompanyManager companyManager;

        public CreditCardManager(
            ICoreContext coreContext, 
            AccountManager accountManager,
            CompanyManager companyManager)
        {
            this.coreContext = coreContext;
            this.accountManager = accountManager;
            this.companyManager = companyManager;
        }
        #endregion

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
                string paymentDescription = string.Format("{0} - {1}{2} ({3} instalment)",
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
                        transaction);
                creditCardPayment.Insert(forceToInsertDb: false);
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

        #region API Implementations

        ICreditCardInfo ICreditCardManager.CreateCreditCard(decimal limit, int extreDate, int type, string validMonth, string validYear, string securityCode, bool isInternetUsageOpen, ICreditCardOwner creditCardOwner)
        {
            return CreateCreditCard(limit, extreDate, type, validMonth, validYear, securityCode, isInternetUsageOpen, creditCardOwner);
        }

        ICreditCardInfo ICreditCardManager.CreateAccountCreditCard(decimal limit, int extreDay, int type, string validMonth, string validYear, string securityCode, bool isInternetUsageOpen, int accountId)
        {
            return CreateAccountCreditCard(limit, extreDay, type, validMonth, validYear, securityCode, isInternetUsageOpen, accountId);
        }

        ICreditCardInfo ICreditCardManager.DoCreditCardPayment(int creditCardId, decimal amount, int instalmentCount, ITransactionOwner toTransactionOwner)
        {
            return DoCreditCardPayment(creditCardId, amount, instalmentCount, toTransactionOwner);
        }

        ICreditCardInfo ICreditCardManager.DoCreditCardPayment(int creditCardId, decimal amount, int instalmentCount, int toAccountId)
        {
            return DoCreditCardPayment(creditCardId, amount, instalmentCount, toAccountId);
        }

        ICreditCardInfo ICreditCardManager.DoCreditCardPayment(string creditCardNumber, string validMonth, string validYear, string securityCode, decimal amount, int instalmentCount, int toAccountId)
        {
            return DoCreditCardPayment(creditCardNumber, validMonth, validYear, securityCode, amount, instalmentCount, toAccountId);
        }
        #endregion
    }
}
