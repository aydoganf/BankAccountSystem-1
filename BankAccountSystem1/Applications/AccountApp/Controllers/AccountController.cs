using AccountApp.Models;
using AccountApp.Models.Operation;
using AccountApp.Utility;
using AydoganFBank.Service.Dispatcher.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountApp.Controllers
{
    public class AccountController : Controller
    {
        #region IoC
        private readonly IAccountManagerService accountManagerService;
        private readonly ICreditCardManagerService creditCardManagerService;

        public AccountController(IAccountManagerService accountManagerService, ICreditCardManagerService creditCardManagerService)
        {
            this.accountManagerService = accountManagerService;
            this.creditCardManagerService = creditCardManagerService;
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Deposit(string accountNumber)
        {
            var account = accountManagerService.GetAccountInfoByAccountNumber(accountNumber);
            var creditCard = creditCardManagerService.GetCreditCardByAccount(accountNumber);

            AccountOverview model = new AccountOverview(account, creditCard);

            return View(model);
        }

        [HttpPost]
        public ActionResult Deposit(string accountNumber, decimal amount)
        {
            var account = accountManagerService.GetAccountInfoByAccountNumber(accountNumber);
            var creditCard = creditCardManagerService.GetCreditCardByAccount(accountNumber);

            try
            {
                account = accountManagerService.DepositToOwnAccount(account.Id, amount);

                AccountOverview model = new AccountOverview(account, creditCard);
                Application.HandleOperation(ViewBag, $"{amount} {model.AssetsUnit} is successfully deposit to your account.");

                return View(model);
            }
            catch (Exception ex)
            {
                AccountOverview model = new AccountOverview(account, creditCard);
                Application.HandleOperation(ex, ViewBag);
                return View(model);
            }            
        }

        public ActionResult Withdraw(string accountNumber)
        {
            var account = accountManagerService.GetAccountInfoByAccountNumber(accountNumber);
            var creditCard = creditCardManagerService.GetCreditCardByAccount(accountNumber);

            AccountOverview model = new AccountOverview(account, creditCard);

            return View(model);
        }

        [HttpPost]
        public ActionResult Withdraw(string accountNumber, decimal amount)
        {
            var account = accountManagerService.GetAccountInfoByAccountNumber(accountNumber);
            var creditCard = creditCardManagerService.GetCreditCardByAccount(accountNumber);

            try
            {
                account = accountManagerService.WithdrawMoneyFromOwn(account.Id, amount);

                AccountOverview model = new AccountOverview(account, creditCard);
                Application.HandleOperation(ViewBag, $"{amount} {model.AssetsUnit} is successfully withrawed from your account.");

                return View(model);
            }
            catch (Exception ex)
            {
                AccountOverview model = new AccountOverview(account, creditCard);
                Application.HandleOperation(ex, ViewBag);
                return View(model);
            }
        }
    }
}