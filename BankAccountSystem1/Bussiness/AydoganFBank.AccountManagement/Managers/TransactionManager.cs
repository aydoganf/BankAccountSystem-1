using AydoganFBank.AccountManagement.Api;
using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Context.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Managers
{
    public class TransactionManager : ITransactionManager
    {
        #region IoC
        private readonly ICoreContext coreContext;
        private readonly AccountManager accountManager;
        private readonly CreditCardManager creditCardManager;

        public TransactionManager(
            ICoreContext coreContext, 
            AccountManager accountManager,
            CreditCardManager creditCardManager)
        {
            this.coreContext = coreContext;
            this.accountManager = accountManager;
            this.creditCardManager = creditCardManager;
        }
        #endregion

        #region CreditCard

        #region ITransactionInfo

        internal List<AccountTransactionDomainEntity> GetCreditCardLastDateRangeTransactionInfoList(int creditCardId, DateTime startDate, DateTime endDate)
        {
            var creditCard = creditCardManager.GetCreditCardById(creditCardId);

            return creditCard.GetLastDateRangeCreditCardTransactionList(startDate, endDate);
        }
        #endregion

        #region ITransactionDetailInfo

        internal List<TransactionDetailDomainEntity> GetCreditCardDateRangeTransactionDetailInfoList(
            int creditCardId, DateTime startDate, DateTime endDate)
        {
            var creditCard = creditCardManager.GetCreditCardById(creditCardId);

            return creditCard.GetTransactionDetailDateRangeList(startDate, endDate);
        }

        internal List<TransactionDetailDomainEntity> GetCreditCardLastDateRangeTransactionDetailInfoList(
            int creditCardId, DateTime startDate, DateTime endDate)
        {
            var creditCard = creditCardManager.GetCreditCardById(creditCardId);

            return creditCard.GetLastTransactionDetailDateRangeList(startDate, endDate);
        }
        #endregion

        #endregion

        #region Account

        #region ITransactionInfo

        internal List<AccountTransactionDomainEntity> GetAccountTransactionDateRangeTransactionInfoList(int accountId, DateTime startDate, DateTime endDate)
        {
            var account = accountManager.GetAccountById(accountId);

            return account.GetLastDateRangeAccountTransactionList(startDate, endDate);
        }

        internal List<AccountTransactionDomainEntity> GetAccountLastIncomingDateRangeAccountTransactionInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            var account = accountManager.GetAccountById(accountId);

            return account.GetLastIncomingDateRangeAccountTransactionList(startDate, endDate);
        }

        internal List<AccountTransactionDomainEntity> GetAccountLastOutgoingDateRangeAccountTransactionInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            var account = accountManager.GetAccountById(accountId);

            return account.GetLastOutgoingDateRangeAccountTransactionList(startDate, endDate);
        }
        #endregion

        #region ITransactionDetailInfo

        internal List<TransactionDetailDomainEntity> GetAccountDateRangeTransactionDetailInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            var account = accountManager.GetAccountById(accountId);

            return account.GetTransactionDetailDateRangeList(startDate, endDate);
        }

        internal List<TransactionDetailDomainEntity> GetAccountLastDateRangeTransactionDetailInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            var account = accountManager.GetAccountById(accountId);

            return account.GetLastTransactionDetailDateRangeList(startDate, endDate);
        }

        internal List<TransactionDetailDomainEntity> GetAccountLastDateRangeAndIncomingTransactionDetailInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            var account = accountManager.GetAccountById(accountId);

            return account.GetLastTransactionDetailDateRangeAndTransactionDirectionList(TransactionDirection.In, startDate, endDate);
        }

        internal List<TransactionDetailDomainEntity> GetAccountLastDateRangeAndOutgoingTransactionDetailInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            var account = accountManager.GetAccountById(accountId);

            return account.GetLastTransactionDetailDateRangeAndTransactionDirectionList(TransactionDirection.Out, startDate, endDate);
        }
        #endregion

        #endregion



        #region API Implementations

        List<ITransactionInfo> ITransactionManager.GetCreditCardLastDateRangeTransactionInfoList(
            int creditCardId, DateTime startDate, DateTime endDate)
        {
            return GetCreditCardLastDateRangeTransactionInfoList(creditCardId, startDate, endDate)
                .Cast<ITransactionInfo>().ToList();
        }

        List<ITransactionInfo> ITransactionManager.GetAccountTransactionDateRangeTransactionInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            return GetAccountTransactionDateRangeTransactionInfoList(accountId, startDate, endDate)
                .Cast<ITransactionInfo>().ToList();
        }

        List<ITransactionInfo> ITransactionManager.GetAccountLastIncomingDateRangeAccountTransactionInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            return GetAccountLastIncomingDateRangeAccountTransactionInfoList(accountId, startDate, endDate)
                .Cast<ITransactionInfo>().ToList();
        }

        List<ITransactionInfo> ITransactionManager.GetAccountLastOutgoingDateRangeAccountTransactionInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            return GetAccountLastOutgoingDateRangeAccountTransactionInfoList(accountId, startDate, endDate)
                .Cast<ITransactionInfo>().ToList();
        }

        List<ITransactionDetailInfo> ITransactionManager.GetCreditCardLastDateRangeTransactionDetailInfoList(
            int creditCardId, DateTime startDate, DateTime endDate)
        {
            return GetCreditCardLastDateRangeTransactionDetailInfoList(creditCardId, startDate, endDate)
                .Cast<ITransactionDetailInfo>().ToList();
        }

        List<ITransactionDetailInfo> ITransactionManager.GetAccountLastDateRangeTransactionDetailInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            return GetAccountLastDateRangeTransactionDetailInfoList(accountId, startDate, endDate)
                .Cast<ITransactionDetailInfo>().ToList();
        }

        List<ITransactionDetailInfo> ITransactionManager.GetCreditCardDateRangeTransactionDetailInfoList(
            int creditCardId, DateTime startDate, DateTime endDate)
        {
            return GetCreditCardDateRangeTransactionDetailInfoList(creditCardId, startDate, endDate)
                .Cast<ITransactionDetailInfo>().ToList();
        }

        List<ITransactionDetailInfo> ITransactionManager.GetAccountDateRangeTransactionDetailInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            return GetAccountDateRangeTransactionDetailInfoList(accountId, startDate, endDate)
                .Cast<ITransactionDetailInfo>().ToList();
        }

        List<ITransactionDetailInfo> ITransactionManager.GetAccountLastDateRangeAndIncomingTransactionDetailInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            return GetAccountLastDateRangeAndIncomingTransactionDetailInfoList(accountId, startDate, endDate)
                .Cast<ITransactionDetailInfo>().ToList();
        }

        List<ITransactionDetailInfo> ITransactionManager.GetAccountLastDateRangeAndOutgoingTransactionDetailInfoList(int accountId, DateTime startDate, DateTime endDate)
        {
            return GetAccountLastDateRangeAndOutgoingTransactionDetailInfoList(accountId, startDate, endDate)
                .Cast<ITransactionDetailInfo>().ToList();
        }


        #endregion
    }
}
