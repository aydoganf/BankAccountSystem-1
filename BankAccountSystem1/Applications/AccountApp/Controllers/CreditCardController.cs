﻿using AccountApp.Models;
using AccountApp.Utility;
using AydoganFBank.Service.Dispatcher.Api;
using AydoganFBank.Service.Dispatcher.Data;
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
            List<TransactionDetailInfo> transactionDetails = new List<TransactionDetailInfo>();

            var extres = creditCardManagerService.GetCreditCardActiveExtreList(creditCard.Id);
            var currentExtre = extres.OrderBy(e => e.Year).ThenBy(e => e.Month).FirstOrDefault();
            var lastExtre = extres.OrderByDescending(e => e.Year).ThenByDescending(e => e.Month).FirstOrDefault();

            if (currentExtre != null && lastExtre != null)
            {
                transactionDetails = creditCardManagerService.GetCreditCardTransactionDetailListByDateRange(
                    creditCard.Id, currentExtre.ExtreStartDate, lastExtre.ExtreEndDate);
            }

            var allPayments = creditCardManagerService.GetCreditCardActivePaymentList(creditCard.Id);
            var currentPayments = allPayments.Where(p => p.InstalmentDate >= currentExtre.ExtreStartDate && p.InstalmentDate <= currentExtre.ExtreEndDate).ToList();
            

            CreditCardOverview overview = new CreditCardOverview(creditCard);
            overview.SetTransantionDetails(transactionDetails);
            overview.SetCurrentExtreCreditCardPaymentList(currentPayments);
            overview.SetCurrentExtre(currentExtre);
            overview.SetCreditCardExtreList(extres);
            overview.SetExtreDetails(extres, allPayments);

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

        public ActionResult Discharge(int id)
        {
            var creditCard = creditCardManagerService.GetCreditCardById(id);
            CreditCardOverview overview = new CreditCardOverview(creditCard);

            return View(overview);
        }
    }
}