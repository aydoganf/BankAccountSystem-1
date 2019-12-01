using AccountApp.Models;
using AccountApp.Models.Operation;
using AccountApp.Models.Operation.CreditCard;
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
    [RoutePrefix("CreditCard")]
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

        [HttpGet]
        [Route("{creditCardId:int}/Detail")]
        public ActionResult Detail(int creditCardId)
        {
            var creditCard = creditCardManagerService.GetCreditCardById(creditCardId);
            List<TransactionDetailInfo> transactionDetails = new List<TransactionDetailInfo>();

            var extres = creditCardManagerService.GetCreditCardActiveExtreList(creditCard.Id);
            var currentExtre = extres.OrderBy(e => e.Year).ThenBy(e => e.Month).FirstOrDefault();
            var lastExtre = extres.OrderByDescending(e => e.Year).ThenByDescending(e => e.Month).FirstOrDefault();

            List<CreditCardPaymentInfo> allPayments = new List<CreditCardPaymentInfo>();
            List<CreditCardPaymentInfo> currentPayments = new List<CreditCardPaymentInfo>();
            List<TransactionDetailInfo> currentTransactionDetails = new List<TransactionDetailInfo>();

            if (currentExtre != null && lastExtre != null)
            {
                transactionDetails = creditCardManagerService.GetCreditCardTransactionDetailListByDateRange(
                    creditCard.Id, currentExtre.ExtreStartDate, lastExtre.ExtreEndDate);

                allPayments = creditCardManagerService.GetCreditCardPaymentList(creditCard.Id, currentExtre.ExtreStartDate, lastExtre.ExtreEndDate);

                currentPayments = allPayments.Where(
                    p =>
                        p.InstalmentDate >= currentExtre.ExtreStartDate &&
                        p.InstalmentDate <= currentExtre.ExtreEndDate).ToList();
                currentTransactionDetails = transactionDetails.Where(
                    td =>
                        td.OccurrenceDate >= currentExtre.ExtreStartDate &&
                        td.OccurrenceDate <= currentExtre.ExtreEndDate).ToList();
            }


            CreditCardOverview overview = new CreditCardOverview(creditCard);
            overview.SetTransantionDetails(transactionDetails);
            overview.SetCurrentExtreCreditCardPaymentList(currentPayments);
            overview.SetCurrentTransactionDetailList(currentTransactionDetails);
            overview.SetCurrentExtre(currentExtre);
            overview.SetCreditCardExtreList(extres);
            overview.SetExtreDetails(extres, allPayments, transactionDetails);

            return View(overview);
        }


        [HttpGet]
        [Route("{creditCardId:int}/DoPayment")]
        public ActionResult DoPayment(int creditCardId)
        {
            var creditCard = creditCardManagerService.GetCreditCardById(creditCardId);
            DoPayment model = new DoPayment();
            model.CreditCardOverview = new CreditCardOverview(creditCard);

            return View(model);
        }

        [HttpPost]
        [Route("{creditCardId:int}/DoPayment")]
        public ActionResult DoPayment(int creditCardId, DoPayment model)
        {
            var creditCard = creditCardManagerService.GetCreditCardById(creditCardId);
            CreditCardOverview overview = new CreditCardOverview(creditCard);
            model.CreditCardOverview = overview;

            try
            {
                var toAccount = accountManagerService.GetAccountInfoByAccountNumber(model.ToAccountNumber);

                overview.CreditCard = creditCardManagerService.DoCreditCardPayment(creditCard.Id, model.Amount, model.InstalmentCount, toAccount.Id);

                Application.HandleOperation(ViewBag, $"{model.Amount} {creditCard.CreditCardOwner.AssetsUnit} is successfully payed to " +
                    $"{toAccount.AccountNumber} - {toAccount.AccountOwner.DisplayName} with {model.InstalmentCount} instalment count.");

                return View(model);
            }
            catch (Exception ex)
            {
                Application.HandleOperation(ex, ViewBag);
                return View(model);
            }
        }

        [HttpGet]
        [Route("{creditCardId:int}/Discharge")]
        public ActionResult Discharge(int creditCardId)
        {
            var creditCard = creditCardManagerService.GetCreditCardById(creditCardId);
            var extre = creditCardManagerService.GetCreditCardCurrentExtre(creditCard.Id);

            var account = accountManagerService.GetAccountInfo(creditCard.CreditCardOwner.OwnerId);

            CreditCardDischarge model = new CreditCardDischarge(creditCard, extre, new List<AccountInfo>() { account });

            return View(model);
        }

        [HttpPost]
        [Route("{creditCardId:int}/Discharge")]
        public ActionResult Discharge(int creditCardId, CreditCardDischarge discharge)
        {
            var creditCard = creditCardManagerService.GetCreditCardById(creditCardId);
            var extre = creditCardManagerService.GetCreditCardCurrentExtre(creditCard.Id);

            var selectedAccount = accountManagerService.GetAccountInfo(discharge.SelectedAccountId);

            var account = accountManagerService.GetAccountInfo(creditCard.CreditCardOwner.OwnerId);
            CreditCardDischarge model = new CreditCardDischarge(creditCard, extre, new List<AccountInfo>() { account });

            try
            {
                extre = creditCardManagerService.DischargeCreditCardExtre(creditCard.Id, discharge.DischargeAmount, selectedAccount.Id);

                Application.HandleOperation(ViewBag, $"Successfully discharged credit card extre for {discharge.DischargeAmount} {creditCard.CreditCardOwner.AssetsUnit}");
                return View(model);
            }
            catch (Exception ex)
            {
                Application.HandleOperation(ex, ViewBag);
                return View(model);
            }
        }

        
        public ActionResult Create()
        {
            var accounts = accountManagerService.GetAccountsByPerson(LoginSession.GetPerson().Id);

            CreditCardCreate model = new CreditCardCreate();
            model.SetAccountList(accounts);

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreditCardCreate model)
        {
            var accounts = accountManagerService.GetAccountsByPerson(LoginSession.GetPerson().Id);
            model.SetAccountList(accounts);

            try
            {
                var securityCode = Application.GenerateCode(3);

                var creditCard = creditCardManagerService.CreateAccountCreditCard(
                    model.Limit,
                    model.ExtreDay,
                    type: 0,
                    model.ValidMonth.ToString(),
                    model.ValidYear.ToString(),
                    securityCode,
                    model.IsInternetUsageOpen,
                    model.AccountId);

                Application.HandleOperation(ViewBag, $"{creditCard.CreditCardMaskedNumber} credit card successfully created.");
                return View(model);
            }
            catch (Exception ex)
            {
                Application.HandleOperation(ex, ViewBag);
                return View(model);
            }
        }
    }
}