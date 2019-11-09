using AccountApp.Models;
using AccountApp.Models.Operation;
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
    public class AccountController : RequiredAuthorizationControllerBase
    {
        #region IoC
        private readonly IAccountManagerService accountManagerService;
        private readonly ICreditCardManagerService creditCardManagerService;
        private readonly ITransactionManagerService transactionManagerService;

        public AccountController(
            IAccountManagerService accountManagerService, 
            ICreditCardManagerService creditCardManagerService,
            ITransactionManagerService transactionManagerService)
        {
            this.accountManagerService = accountManagerService;
            this.creditCardManagerService = creditCardManagerService;
            this.transactionManagerService = transactionManagerService;
        }
        #endregion


        #region Model utility
        private AccountOverview CreateAccountOverview(string accountNumber)
        {
            var account = accountManagerService.GetAccountInfoByAccountNumber(accountNumber);
            var creditCard = creditCardManagerService.GetCreditCardByAccount(accountNumber);

            return new AccountOverview(account, creditCard);
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Deposit(string accountNumber)
        {
            AccountOverview model = CreateAccountOverview(accountNumber);

            return View(model);
        }

        [HttpPost]
        public ActionResult Deposit(string accountNumber, decimal amount)
        {
            AccountOverview overview = CreateAccountOverview(accountNumber);

            try
            {
                overview.Account = accountManagerService.DepositToOwnAccount(overview.Account.Id, amount);
                Application.HandleOperation(ViewBag, $"{amount} {overview.AssetsUnit} is successfully deposit to your account.");

                return View(overview);
            }
            catch (Exception ex)
            {
                Application.HandleOperation(ex, ViewBag);
                return View(overview);
            }            
        }

        public ActionResult Withdraw(string accountNumber)
        {
            AccountOverview model = CreateAccountOverview(accountNumber);

            return View(model);
        }

        [HttpPost]
        public ActionResult Withdraw(string accountNumber, decimal amount)
        {
            AccountOverview overview = CreateAccountOverview(accountNumber);

            try
            {
                overview.Account = accountManagerService.WithdrawMoneyFromOwn(overview.Account.Id, amount);

                Application.HandleOperation(ViewBag, $"{amount} {overview.AssetsUnit} is successfully withrawed from your account.");

                return View(overview);
            }
            catch (Exception ex)
            {
                Application.HandleOperation(ex, ViewBag);
                return View(overview);
            }
        }

        public ActionResult Detail(string accountNumber)
        {
            AccountOverview overview = CreateAccountOverview(accountNumber);

            var transactionDetails = transactionManagerService.GetAccountLastDateRangeTransactionDetailInfoList(
                overview.Account.Id,
                DateTime.Now.AddDays(-30),
                DateTime.Now);

            overview.SetTransactionDetailList(transactionDetails);

            return View(overview);
        }

        public ActionResult TransferAssets(string accountNumber)
        {
            AccountOverview overview = CreateAccountOverview(accountNumber);

            return View(overview);
        }

        [HttpPost]
        public ActionResult TransferAssets(string accountNumber, string toAccountNumber, decimal amount)
        {
            AccountOverview overview = CreateAccountOverview(accountNumber);
 
            try
            {
                AccountInfo toAccount = accountManagerService.GetAccountInfoByAccountNumber(toAccountNumber);

                var obj = accountManagerService.TransferAssets(
                    overview.Account.Id,
                    toAccount.Id,
                    amount,
                    AydoganFBank.AccountManagement.Api.TransactionTypeEnum.FromAccountToAccount);

                overview.Account.Balance -= amount;

                Application.HandleOperation(ViewBag, 
                    $"{amount} {overview.AssetsUnit} is successfully transferred to account {toAccount.AccountNumber} - {toAccount.AccountOwner.DisplayName}.");

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