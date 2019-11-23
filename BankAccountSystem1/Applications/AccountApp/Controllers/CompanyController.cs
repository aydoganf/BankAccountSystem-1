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
    public class CompanyController : RequiredAuthorizationControllerBase
    {
        private readonly ICompanyManagerService companyManagerService;

        public CompanyController(ICompanyManagerService companyManagerService)
        {
            this.companyManagerService = companyManagerService;
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
    }
}