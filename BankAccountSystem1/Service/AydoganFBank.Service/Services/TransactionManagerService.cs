using AydoganFBank.AccountManagement.Service;
using AydoganFBank.Service.Builder;
using AydoganFBank.Service.Message.Data;
using AydoganFBank.Service.Services.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Services
{
    public class TransactionManagerService : ITransactionManagerService
    {
        private readonly ITransactionManager transactionManager;
        private readonly ServiceDataBuilder dataBuilder;

        public TransactionManagerService(ITransactionManager transactionManager, ServiceDataBuilder dataBuilder)
        {
            this.transactionManager = transactionManager;
            this.dataBuilder = dataBuilder;
        }

        public List<TransactionInfo> GetAccountLastIncomingDateRangeAccountTransactionInfoList(int accountId, DateTime startDate, DateTime endDate)
        {
            var list = transactionManager.GetAccountLastIncomingDateRangeAccountTransactionInfoList(accountId, startDate, endDate);
            return dataBuilder.BuildTransactionInfoList(list);
        }

        public List<TransactionInfo> GetAccountLastOutgoingDateRangeAccountTransactionInfoList(int accountId, DateTime startDate, DateTime endDate)
        {
            var list = transactionManager.GetAccountLastOutgoingDateRangeAccountTransactionInfoList(accountId, startDate, endDate);
            return dataBuilder.BuildTransactionInfoList(list);
        }

        public List<TransactionInfo> GetAccountTransactionDateRangeTransactionInfoList(int accountId, DateTime startDate, DateTime endDate)
        {
            var list = transactionManager.GetAccountTransactionDateRangeTransactionInfoList(accountId, startDate, endDate);
            return dataBuilder.BuildTransactionInfoList(list);
        }

        public List<TransactionInfo> GetCreditCardLastDateRangeTransactionInfoList(int creditCardId, DateTime startDate, DateTime endDate)
        {
            var list = transactionManager.GetCreditCardLastDateRangeTransactionInfoList(creditCardId, startDate, endDate);
            return dataBuilder.BuildTransactionInfoList(list);
        }
    }
}
