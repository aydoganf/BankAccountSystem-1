using AccountApp.Models;
using AccountApp.Utility;
using AydoganFBank.Service.Dispatcher.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountApp.Controllers
{
    public class CreditCardController : RequiredAuthorizationControllerBase
    {
        #region IoC
        private readonly ICreditCardManagerService creditCardManagerService;
        private readonly ITransactionManagerService transactionManagerService;
        private readonly IAccountManagerService accountManagerService;

        public CreditCardController(
            ICreditCardManagerService creditCardManagerService, 
            ITransactionManagerService transactionManagerService,
            IAccountManagerService accountManagerService)
        {
            this.creditCardManagerService = creditCardManagerService;
            this.transactionManagerService = transactionManagerService;
            this.accountManagerService = accountManagerService;
        }
        #endregion

        public ActionResult Detail(int id)
        {
            var creditCard = creditCardManagerService.GetCreditCardById(id);

            DateTime now = DateTime.Now;
            DateTime beforeExtreDate = now;
            if (DateTime.Now.Day < creditCard.ExtreDay)
                beforeExtreDate = new DateTime(now.Year, now.Month - 1, creditCard.ExtreDay);
            else
                beforeExtreDate = new DateTime(now.Year, now.Month, creditCard.ExtreDay);


            var payments = creditCardManagerService.GetCreditCardLastExtrePayments(creditCard.Id);
            //var transactions = transactionManagerService.GetCreditCardLastDateRangeTransactionDetailInfoList(
            //    creditCard.Id, 
            //    beforeExtreDate, 
            //    DateTime.Now);

            CreditCardOverview overview = new CreditCardOverview(creditCard);
            //overview.SetTransantionDetails(transactions);
            overview.SetCreditCardPaymentList(payments);

            return View(overview);
        }

        public ActionResult DoPayment(int id)
        {
            var creditCard = creditCardManagerService.GetCreditCardById(id);
            CreditCardOverview overview = new CreditCardOverview(creditCard);

            return View(overview);
        }

        [HttpPost]
        public ActionResult DoPayment(int id, string toAccountNumber, decimal amount, int instalmentCount)
        {
            var creditCard = creditCardManagerService.GetCreditCardById(id);
            var toAccount = accountManagerService.GetAccountInfoByAccountNumber(toAccountNumber);

            creditCard = creditCardManagerService.DoCreditCardPayment(creditCard.Id, amount, instalmentCount, toAccount.Id);
            
            CreditCardOverview overview = new CreditCardOverview(creditCard);

            return View(overview);
        }
    }
}