using AydoganFBank.Service.Message.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Services.APIs
{
    public interface ITransactionManagerService
    {
        List<TransactionInfo> GetCreditCardLastDateRangeTransactionInfoList(
            int creditCardId, 
            DateTime startDate, 
            DateTime endDate);

        List<TransactionInfo> GetAccountTransactionDateRangeTransactionInfoList(
            int accountId, 
            DateTime startDate, 
            DateTime endDate);

        List<TransactionInfo> GetAccountLastIncomingDateRangeAccountTransactionInfoList(
            int accountId,
            DateTime startDate,
            DateTime endDate);

        List<TransactionInfo> GetAccountLastOutgoingDateRangeAccountTransactionInfoList(
            int accountId,
            DateTime startDate,
            DateTime endDate);
    }
}
