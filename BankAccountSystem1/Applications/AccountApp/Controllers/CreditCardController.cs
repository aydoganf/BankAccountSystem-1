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

            var payments = creditCardManagerService.GetCreditCardLastExtrePayments(creditCard.Id);
            var extres = creditCardManagerService.GetCreditCardActiveExtreList(creditCard.Id);

            CreditCardOverview overview = new CreditCardOverview(creditCard);
            overview.SetCreditCardPaymentList(payments);
            overview.SetCreditCardExtreList(extres);

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
            CreditCardOverview overview = new CreditCardOverview(creditCard);

            try
            {
                var toAccount = accountManagerService.GetAccountInfoByAccountNumber(toAccountNumber);

                overview.CreditCard = creditCardManagerService.DoCreditCardPayment(creditCard.Id, amount, instalmentCount, toAccount.Id);

                Application.HandleOperation(ViewBag, $"{amount} {creditCard.CreditCardOwner.AssetsUnit} is successfully payed to " +
                    $"{toAccount.AccountNumber} - {toAccount.AccountOwner.DisplayName} with {instalmentCount} instalment count.");

                return View(overview);
            }
            catch (Exception ex)
            {
                Application.HandleOperation(ex, ViewBag);
                return View(overview);
            }
        }
    }
}