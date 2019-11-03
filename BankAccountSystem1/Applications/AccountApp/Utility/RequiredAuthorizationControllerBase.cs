using AccountApp.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountApp.Utility
{
    [AuthorizationFilter]
    public class RequiredAuthorizationControllerBase : Controller
    {
        private Session session;
        public Session LoginSession
        {
            get
            {
                if (session == null)
                {
                    session = HttpContext.GetSession();
                }
                return session;
            }
        }
    }
}