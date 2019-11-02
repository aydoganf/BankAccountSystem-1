using AydoganFBank.Service.Dispatcher.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountApp.Controllers
{
    public class HomeController : Controller
    {
        private IAccountManagerService accountManagerService;
        private ISecurityManagerService securityManagerService;

        public HomeController(IAccountManagerService accountManagerService, ISecurityManagerService securityManagerService)
        {
            this.accountManagerService = accountManagerService;
            this.securityManagerService = securityManagerService;
        }

        public ActionResult Index()
        {
            var data = accountManagerService.GetAccountInfo(1);
            return View(data);
        }

        public ActionResult About()
        {
            var token = securityManagerService.GetTokenByValueAndApplication("faruk", 1);
            return View(token);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}