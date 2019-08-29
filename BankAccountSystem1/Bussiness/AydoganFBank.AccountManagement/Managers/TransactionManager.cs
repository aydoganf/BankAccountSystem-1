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

        public TransactionManager(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }
        #endregion

        internal List<ITransactionInfo> GetCreditCardLastDateRangeTransactionInfoList(int creditCardId, DateTime startDate, DateTime endDate)
        {
            var creditCard = coreContext.Query<ICreditCardRepository>()
                .GetById(creditCardId);

            return creditCard.GetLastDateRangeCreditCardTransactionList(startDate, endDate).Cast<ITransactionInfo>().ToList();
        }

        internal List<ITransactionInfo> GetAccountTransactionDateRangeTransactionInfoList(int accountId, DateTime startDate, DateTime endDate)
        {
            var account = coreContext.Query<IAccountRepository>()
                .GetById(accountId);

            return account.GetLastDateRangeAccountTransactionList(startDate, endDate).Cast<ITransactionInfo>().ToList();
        }

        internal List<ITransactionDetailInfo> GetCreditCardDateRangeTransactionDetailInfoList(
            int creditCardId, DateTime startDate, DateTime endDate)
        {
            var creditCard = coreContext.Query<ICreditCardRepository>()
                .GetById(creditCardId);

            return creditCard.GetTransactionDetailDateRangeList(startDate, endDate).Cast<ITransactionDetailInfo>().ToList();
        }

        internal List<ITransactionDetailInfo> GetCreditCardLastDateRangeTransactionDetailInfoList(
            int creditCardId, DateTime startDate, DateTime endDate)
        {
            var creditCard = coreContext.Query<ICreditCardRepository>()
                .GetById(creditCardId);

            return creditCard.GetLastTransactionDetailDateRangeList(startDate, endDate).Cast<ITransactionDetailInfo>().ToList();
        }

        internal List<ITransactionDetailInfo> GetAccountDateRangeTransactionDetailInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            var account = coreContext.Query<IAccountRepository>()
                .GetById(accountId);

            return account.GetTransactionDetailDateRangeList(startDate, endDate).Cast<ITransactionDetailInfo>().ToList();
        }

        internal List<ITransactionDetailInfo> GetAccountLastDateRangeTransactionDetailInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            var account = coreContext.Query<IAccountRepository>()
                .GetById(accountId);

            return account.GetLastTransactionDetailDateRangeList(startDate, endDate).Cast<ITransactionDetailInfo>().ToList();
        }

        #region API Implementations

        List<ITransactionInfo> ITransactionManager.GetCreditCardLastDateRangeTransactionInfoList(
            int creditCardId, DateTime startDate, DateTime endDate)
        {
            return GetCreditCardLastDateRangeTransactionInfoList(creditCardId, startDate, endDate);
        }

        List<ITransactionInfo> ITransactionManager.GetAccountTransactionDateRangeTransactionInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            return GetAccountTransactionDateRangeTransactionInfoList(accountId, startDate, endDate);
        }

        List<ITransactionDetailInfo> ITransactionManager.GetCreditCardLastDateRangeTransactionDetailInfoList(
            int creditCardId, DateTime startDate, DateTime endDate)
        {
            return GetCreditCardLastDateRangeTransactionDetailInfoList(creditCardId, startDate, endDate);
        }

        List<ITransactionDetailInfo> ITransactionManager.GetAccountLastDateRangeTransactionDetailInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            return GetAccountLastDateRangeTransactionDetailInfoList(accountId, startDate, endDate);
        }

        List<ITransactionDetailInfo> ITransactionManager.GetCreditCardDateRangeTransactionDetailInfoList(
            int creditCardId, DateTime startDate, DateTime endDate)
        {
            return GetCreditCardDateRangeTransactionDetailInfoList(creditCardId, startDate, endDate);
        }

        List<ITransactionDetailInfo> ITransactionManager.GetAccountDateRangeTransactionDetailInfoList(
            int accountId, DateTime startDate, DateTime endDate)
        {
            return GetAccountDateRangeTransactionDetailInfoList(accountId, startDate, endDate);
        }
        #endregion
    }
}
