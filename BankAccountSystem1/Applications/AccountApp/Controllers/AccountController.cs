using AccountApp.Models;
using AccountApp.Models.Operation;
using AccountApp.Models.Operation.Account;
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
    [RoutePrefix("Account")]
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

        private AccountOverview CreateAccountOverview(int accountId)
        {
            var account = accountManagerService.GetAccountInfo(accountId);
            var creditCard = creditCardManagerService.GetCreditCardByAccount(account.AccountNumber);

            return new AccountOverview(account, creditCard);
        }
        #endregion

        [HttpGet]
        [Route("{accountId:int}/Deposit")]
        public ActionResult Deposit(int accountId)
        {
            Deposit model = new Deposit();
            model.AccountOverview = CreateAccountOverview(accountId);

            return View(model);
        }

        [HttpPost]
        [Route("{accountId:int}/Deposit")]
        public ActionResult Deposit(int accountId, Deposit model)
        {
            AccountOverview overview = CreateAccountOverview(accountId);
            model.AccountOverview = overview;

            try
            {
                overview.Account = accountManagerService.DepositToOwnAccount(overview.Account.Id, model.Amount);
                Application.HandleOperation(ViewBag, $"{model.Amount} {overview.AssetsUnit} is successfully deposit to your account.");

                return View(model);
            }
            catch (Exception ex)
            {
                Application.HandleOperation(ex, ViewBag);
                return View(model);
            }            
        }

        [HttpGet]
        [Route("{accountId:int}/Withdraw")]
        public ActionResult Withdraw(int accountId)
        {
            Withdraw model = new Withdraw();
            model.AccountOverview = CreateAccountOverview(accountId);

            return View(model);
        }

        [HttpPost]
        [Route("{accountId:int}/Withdraw")]
        public ActionResult Withdraw(int accountId, Withdraw model)
        {
            AccountOverview overview = CreateAccountOverview(accountId);
            model.AccountOverview = overview;

            try
            {
                overview.Account = accountManagerService.WithdrawMoneyFromOwn(overview.Account.Id, model.Amount);

                Application.HandleOperation(ViewBag, $"{model.Amount} {overview.AssetsUnit} is successfully withrawed from your account.");
                
                return View(model);
            }
            catch (Exception ex)
            {
                Application.HandleOperation(ex, ViewBag);
                return View(model);
            }
        }

        [HttpGet]
        [Route("{accountId:int}/Detail")]
        public ActionResult Detail(int accountId)
        {
            AccountOverview overview = CreateAccountOverview(accountId);

            var transactionDetails = transactionManagerService.GetAccountLastDateRangeTransactionDetailInfoList(
                overview.Account.Id,
                DateTime.Now.AddDays(-30),
                DateTime.Now);

            overview.SetTransactionDetailList(transactionDetails);

            return View(overview);
        }

        [HttpGet]
        [Route("{accountId:int}/TransferAssets")]
        public ActionResult TransferAssets(int accountId)
        {
            TransferAssets model = new TransferAssets();
            model.AccountOverview = CreateAccountOverview(accountId);

            return View(model);
        }

        [HttpPost]
        [Route("{accountId:int}/TransferAssets")]
        public ActionResult TransferAssets(int accountId, TransferAssets model)
        {
            AccountOverview overview = CreateAccountOverview(accountId);
            model.AccountOverview = overview;

            try
            {
                AccountInfo toAccount = accountManagerService.GetAccountInfoByAccountNumber(model.ToAccountNumber);

                var obj = accountManagerService.TransferAssets(
                    overview.Account.Id,
                    toAccount.Id,
                    model.Amount,
                    AydoganFBank.AccountManagement.Api.TransactionTypeEnum.FromAccountToAccount);

                overview.Account.Balance -= model.Amount;

                Application.HandleOperation(ViewBag, 
                    $"{model.Amount} {overview.AssetsUnit} is successfully transferred to account {toAccount.AccountNumber} - {toAccount.AccountOwner.DisplayName}.");

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
            var accountTypes = accountManagerService.GetAccountTypeList();

            AccountCreate model = new AccountCreate();
            model.SetAccountTypeList(accountTypes);

            TempData["AccountTypeList"] = accountTypes;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AccountCreate model)
        {
            var accountTypes = accountManagerService.GetAccountTypeList();
            model.SetAccountTypeList(accountTypes);

            if (model.AccountTypeId == 0)
            {
                Application.HandleWarning(ViewBag, "Please select an account type to create new account.");
                return View(model);
            }

            try
            {
                var accountType = accountManagerService.GetAccountTypeInfo(model.AccountTypeId);
                var account = accountManagerService.CreatePersonAccount(accountType.TypeKey, LoginSession.GetPerson().Id);

                Application.HandleOperation(ViewBag, $"Your account {account.AccountNumber} has been created successfully!");

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