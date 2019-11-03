using AccountApp.Utility;
using AydoganFBank.Service.Dispatcher.Api;
using AydoganFBank.Service.Dispatcher.Data;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISecurityManagerService securityManagerService;

        public LoginController(ISecurityManagerService securityManagerService)
        {
            this.securityManagerService = securityManagerService;
        }

        // GET: Login
        public ActionResult Index()
        {
            Session session = HttpContext.GetSession();
            if (session.IsTokenExistsAndValid)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(string identity, string password)
        {
            Session session = HttpContext.GetSession();
            if (session.IsTokenExistsAndValid)
            {
                return RedirectToAction("Index", "Home");
            }

            TokenInfo token = null;
            dynamic errorResult = new ExpandoObject();
            try
            {
                token = securityManagerService.Login(identity, password, Application.APPLICATION_ID);
            }
            catch (Exception ex)
            {
                if (ex is AydoganFBank.Context.Exception.ServiceException)
                    errorResult.Message = ((AydoganFBank.Context.Exception.ServiceException)ex).ExceptionMessage;
                else
                    errorResult.Message = "Something went wrong. Try again!";

                ViewBag.ErrorResult = errorResult;
                return View();
            }

            if (token == null)
            {
                errorResult.Message = "Somethings went wrong. Try again!";
                ViewBag.ErrorResult = errorResult;
                return View();
            }

            session.SetToken(token);
            return RedirectToAction("Index", "Home");
        }
    }
}