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
    [RoutePrefix("Company")]
    public class CompanyController : RequiredAuthorizationControllerBase
    {
        private readonly ICompanyManagerService companyManagerService;
        private readonly ICreditCardManagerService creditCardManagerService;

        public CompanyController(
            ICompanyManagerService companyManagerService,
            ICreditCardManagerService creditCardManagerService)
        {
            this.companyManagerService = companyManagerService;
            this.creditCardManagerService = creditCardManagerService;
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create()
        {
            return View(new CompanyCreate());
        }

        [HttpPost]
        public ActionResult Create(CompanyCreate model)
        {
            try
            {
                var company = companyManagerService.CreateCompany(model.CompanyName, LoginSession.GetPerson().Id, model.Address, model.PhoneNumber, model.TaxNumber);

                Application.HandleOperation(ViewBag, $"{company.CompanyName} company successfully created.");

                return View(model);
            }
            catch (Exception ex)
            {
                Application.HandleOperation(ex, ViewBag);
                return View(model);
            }
        }

        
        public ActionResult Detail(int id)
        {
            var company = companyManagerService.GetCompanyInfo(id);
            var accounts = companyManagerService.GetCompanyAccounts(company.Id);

            List<CreditCardInfo> creditCards = new List<CreditCardInfo>();
            foreach (var account in accounts)
            {
                creditCards.Add(creditCardManagerService.GetCreditCardByAccount(account.AccountNumber));
            }

            CompanyDetail model = new CompanyDetail(company, accounts, creditCards); 

            return View(model);
        }


        [HttpGet]
        [Route("{companyId:int}/ChangeAddress")]
        public ActionResult ChangeAddress(int companyId)
        {
            var company = companyManagerService.GetCompanyInfo(companyId);


            CompanyAddressChange model = new CompanyAddressChange(company.CompanyName, company.Address);

            return View(model);
        }

        [HttpPost]
        [Route("{companyId:int}/ChangeAddress")]
        public ActionResult ChangeAddress(int companyId, CompanyAddressChange model)
        {
            try
            {
                var company = companyManagerService.ChangeCompanyAddress(companyId, model.Address);

                model.CompanyName = company.CompanyName;

                Application.HandleOperation(ViewBag, $"{company.CompanyName} company address successfully updated.");
                return View(model);
            }
            catch (Exception ex)
            {
                Application.HandleOperation(ex, ViewBag);
                return View(model);
            }            
        }

        [HttpGet]
        [Route("{companyId:int}/ChangePhoneNumber")]
        public ActionResult ChangePhoneNumber(int companyId)
        {
            var company = companyManagerService.GetCompanyInfo(companyId);

            CompanyPhoneNumberChange model = new CompanyPhoneNumberChange(company.CompanyName, company.PhoneNumber);

            return View(model);
        }

        [HttpPost]
        [Route("{companyId:int}/ChangePhoneNumber")]
        public ActionResult ChangePhoneNumber(int companyId, CompanyPhoneNumberChange model)
        {
            try
            {
                var company = companyManagerService.ChangeCompanyPhoneNumber(companyId, model.PhoneNumber);

                model.CompanyName = company.CompanyName;

                Application.HandleOperation(ViewBag, $"{company.CompanyName} company phone number successfully updated.");
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