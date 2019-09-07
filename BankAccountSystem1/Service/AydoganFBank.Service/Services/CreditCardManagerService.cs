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
    public class CreditCardManagerService : ICreditCardManagerService
    {
        private readonly ICreditCardManager creditCardManager;
        private readonly ServiceDataBuilder dataBuilder;

        public CreditCardManagerService(ICreditCardManager creditCardManager, ServiceDataBuilder dataBuilder)
        {
            this.creditCardManager = creditCardManager;
            this.dataBuilder = dataBuilder;
        }

        public CreditCardInfo CreateAccountCreditCard(
            decimal limit, int extreDay, int type, string validMonth, string validYear, 
            string securityCode, bool isInternetUsageOpen, int accountId)
        {
            var creditCard = creditCardManager.CreateAccountCreditCard(
                limit, extreDay, type, validMonth, validYear, securityCode, isInternetUsageOpen, accountId);
            return dataBuilder.BuildCreditCardInfo(creditCard);
        }

        public CreditCardInfo DoCreditCardPayment(int creditCardId, decimal amount, int instalmentCount, int toAccountId)
        {
            var creditCard = creditCardManager.DoCreditCardPayment(creditCardId, amount, instalmentCount, toAccountId);
            return dataBuilder.BuildCreditCardInfo(creditCard);
        }

        public CreditCardInfo DoCreditCardPayment(
            string creditCardNumber, string validMonth, string validYear, string securityCode, 
            decimal amount, int instalmentCount, int toAccountId)
        {
            var creditCard = creditCardManager.DoCreditCardPayment(
                creditCardNumber, validMonth, validYear, securityCode, amount, instalmentCount, toAccountId);
            return dataBuilder.BuildCreditCardInfo(creditCard);
        }
    }
}
