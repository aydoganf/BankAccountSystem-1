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

        public HomeController(IAccountManagerService accountManagerService)
        {
            this.accountManagerService = accountManagerService;
        }

        public ActionResult Index()
        {
            var data = accountManagerService.GetAccountInfo(1);
            return View(data);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}