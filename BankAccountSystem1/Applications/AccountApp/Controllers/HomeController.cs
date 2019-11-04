using AccountApp.Filters;
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
    
    public class HomeController : RequiredAuthorizationControllerBase
    {
        #region IoC

        private readonly IAccountManagerService accountManagerService;
        private readonly ICreditCardManagerService creditCardManagerService;

        public HomeController(IAccountManagerService accountManagerService, ICreditCardManagerService creditCardManagerService)
        {
            this.accountManagerService = accountManagerService;
            this.creditCardManagerService = creditCardManagerService;
        }

        #endregion

        public ActionResult Index()
        {
            var person = LoginSession.GetPerson();

            var accounts = accountManagerService.GetAccountsByPerson(person.Id);
            var creditCards = creditCardManagerService.GetCreditCardListByPerson(person.Id);

            PersonOverview personOverview = new PersonOverview(accounts, creditCards);

            return View(personOverview);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}