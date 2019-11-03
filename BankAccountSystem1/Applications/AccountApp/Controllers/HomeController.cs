using AccountApp.Filters;
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

        private IAccountManagerService accountManagerService;

        public HomeController(IAccountManagerService accountManagerService)
        {
            this.accountManagerService = accountManagerService;
        }

        #endregion

        public ActionResult Index()
        {
            var data = accountManagerService.GetAccountsByPerson(LoginSession.GetPerson().Id);
            return View(data);
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