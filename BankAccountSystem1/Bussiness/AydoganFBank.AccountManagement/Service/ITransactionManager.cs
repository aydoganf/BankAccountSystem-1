using AydoganFBank.AccountManagement.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Service
{
    public interface ITransactionManager
    {
        List<ITransactionInfo> GetCreditCardLastDateRangeTransactionInfoList(int creditCardId, DateTime startDate, DateTime endDate);
        List<ITransactionInfo> GetAccountTransactionDateRangeTransactionInfoList(int accountId, DateTime startDate, DateTime endDate);

        List<ITransactionDetailInfo> GetCreditCardLastDateRangeTransactionDetailInfoList(
            int creditCardId, 
            DateTime startDate, 
            DateTime endDate);

        List<ITransactionDetailInfo> GetAccountLastDateRangeTransactionDetailInfoList(
            int accountId, 
            DateTime startDate, 
            DateTime endDate);

        List<ITransactionDetailInfo> GetCreditCardDateRangeTransactionDetailInfoList(
            int creditCardId, 
            DateTime startDate, 
            DateTime endDate);

        List<ITransactionDetailInfo> GetAccountDateRangeTransactionDetailInfoList(
            int accountId, 
            DateTime startDate, 
            DateTime endDate);

        List<ITransactionInfo> GetAccountLastIncomingDateRangeAccountTransactionInfoList(
            int accountId, 
            DateTime startDate, 
            DateTime endDate);

        List<ITransactionInfo> GetAccountLastOutgoingDateRangeAccountTransactionInfoList(
            int accountId, 
            DateTime startDate, 
            DateTime endDate);

        List<ITransactionDetailInfo> GetAccountLastDateRangeAndIncomingTransactionDetailInfoList(
            int accountId, 
            DateTime startDate, 
            DateTime endDate);

        List<ITransactionDetailInfo> GetAccountLastDateRangeAndOutgoingTransactionDetailInfoList(
            int accountId, 
            DateTime startDate, 
            DateTime endDate);
    }
}
