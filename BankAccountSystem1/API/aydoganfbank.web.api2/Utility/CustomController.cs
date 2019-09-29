using AydoganFBank.Context.IoC;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aydoganfbank.web.api2.Utility;

namespace aydoganfbank.web.api2.Utility
{
    public class CustomController : ControllerBase
    {
        private readonly ICoreContext coreContext;

        public CustomController(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
            string auth = Request.GetAuthenticationHeader();
            


        }


    }
}
