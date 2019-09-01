using AydoganFBank.Service.Message.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Services.APIs
{
    public interface ICreditCardManagerService
    {

        CreditCardInfo DoCreditCardPayment(
            int creditCardId,
            decimal amount,
            int instalmentCount,
            int toAccountId);

        CreditCardInfo CreateAccountCreditCard(
            decimal limit,
            int extreDate,
            int type,
            string validMonth,
            string validYear,
            string securityCode,
            bool isInternetUsageOpen,
            int accountId);

        CreditCardInfo CreateCompanyCreditCard(
            decimal limit,
            int extreDate,
            int type,
            string validMonth,
            string validYear,
            string securityCode,
            bool isInternetUsageOpen,
            int companyId);

        CreditCardInfo DoCreditCardPayment(
            string creditCardNumber,
            string validMonth,
            string validYear,
            string securityCode,
            decimal amount,
            int instalmentCount,
            int toAccountId);
    }
}
