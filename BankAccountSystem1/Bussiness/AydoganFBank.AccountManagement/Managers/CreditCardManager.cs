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

        public CreditCardManager(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }
        #endregion

        private CreditCardDomainEntity CreateCreditCard(decimal limit, int extreDate, int type, string validMonth, string validYear, string securityCode,
            bool isInternetUsageOpen, ICreditCardOwner creditCardOwner)
        {
            var creditCard = coreContext.New<CreditCardDomainEntity>()
                .With(limit, extreDate, type, validMonth, validYear, securityCode, isInternetUsageOpen, creditCardOwner);
            creditCard.Insert();

            return creditCard;
        }

        internal CreditCardDomainEntity CreateAccountCreditCard(decimal limit, int extreDate, int type, string validMonth, string validYear, string securityCode, bool isInternetUsageOpen, int accountId)
        {
            var account = coreContext.Query<IAccountRepository>()
                .GetById(accountId);

            return CreateCreditCard(limit, extreDate, type, validMonth, validYear, securityCode, isInternetUsageOpen, account);
        }

        internal CreditCardDomainEntity CreateCompanyCreditCard(decimal limit, int extreDate, int type, string validMonth, string validYear, string securityCode, bool isInternetUsageOpen, int companyId)
        {
            var company = coreContext.Query<ICompanyRepository>()
                .GetById(companyId);

            return CreateCreditCard(limit, extreDate, type, validMonth, validYear, securityCode, isInternetUsageOpen, company);
        }

        private CreditCardDomainEntity DoCreditCardPayment(CreditCardDomainEntity creditCard, decimal amount, int instalmentCount, ITransactionOwner toTransactionOwner)
        {
            creditCard.DoPayment(amount);

            var transaction = coreContext.New<AccountTransactionDomainEntity>()
                .With(creditCard, toTransactionOwner, amount, TransactionTypeEnum.CreditCardPayment, TransactionStatusEnum.InProgress, creditCard);

            transaction.Insert();
            var transactionDetail = transaction.CreateTransactionDetail(TransactionDirection.Out);
            transactionDetail.Insert();

            for (int instalmentIndex = 1; instalmentIndex <= instalmentCount; instalmentIndex++)
            {
                decimal instalmentAmount = amount / instalmentCount;

                string paymentDescription = string.Format("{0} - {1}{2} ({3} instalment)",
                    toTransactionOwner.TransactionDetailDisplayName, instalmentAmount, toTransactionOwner.AssetsUnit,
                    string.Format("{0}/{1}", instalmentIndex, instalmentCount));

                DateTime instalmentDate = transaction.TransactionDate.AddMonths(instalmentIndex - 1);

                var creditCardPayment = coreContext.New<CreditCardPaymentDomainEntity>()
                    .With(instalmentIndex, instalmentAmount, paymentDescription,
                    transaction.TransactionDate, instalmentDate, transaction);
                creditCardPayment.Insert();
            }

            return creditCard;
        }

        internal CreditCardDomainEntity DoCreditCardPayment(int creditCardId, decimal amount, int instalmentCount, ITransactionOwner toTransactionOwner)
        {
            var creditCard = coreContext.Query<ICreditCardRepository>()
                .GetById(creditCardId);

            return DoCreditCardPayment(creditCard, amount, instalmentCount, toTransactionOwner);
        }

        internal CreditCardDomainEntity DoCreditCardPayment(int creditCardId, decimal amount, int instalmentCount, int toAccountId)
        {
            var creditCard = coreContext.Query<ICreditCardRepository>()
                .GetById(creditCardId);

            var toAccount = coreContext.Query<IAccountRepository>()
                .GetById(toAccountId);

            return DoCreditCardPayment(creditCard, amount, instalmentCount, toAccount);
        }


        #region API Implementations

        ICreditCardInfo ICreditCardManager.CreateCreditCard(decimal limit, int extreDate, int type, string validMonth, string validYear, string securityCode, bool isInternetUsageOpen, ICreditCardOwner creditCardOwner)
        {
            return CreateCreditCard(limit, extreDate, type, validMonth, validYear, securityCode, isInternetUsageOpen, creditCardOwner);
        }

        ICreditCardInfo ICreditCardManager.CreateAccountCreditCard(decimal limit, int extreDate, int type, string validMonth, string validYear, string securityCode, bool isInternetUsageOpen, int accountId)
        {
            return CreateAccountCreditCard(limit, extreDate, type, validMonth, validYear, securityCode, isInternetUsageOpen, accountId);
        }

        ICreditCardInfo ICreditCardManager.CreateCompanyCreditCard(decimal limit, int extreDate, int type, string validMonth, string validYear, string securityCode, bool isInternetUsageOpen, int companyId)
        {
            return CreateCompanyCreditCard(limit, extreDate, type, validMonth, validYear, securityCode, isInternetUsageOpen, companyId);
        }

        ICreditCardInfo ICreditCardManager.DoCreditCardPayment(int creditCardId, decimal amount, int instalmentCount, ITransactionOwner toTransactionOwner)
        {
            return DoCreditCardPayment(creditCardId, amount, instalmentCount, toTransactionOwner);
        }

        ICreditCardInfo ICreditCardManager.DoCreditCardPayment(int creditCardId, decimal amount, int instalmentCount, int toAccountId)
        {
            return DoCreditCardPayment(creditCardId, amount, instalmentCount, toAccountId);
        }
        #endregion
    }
}
