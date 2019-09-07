using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aydoganfbank.web.api.bussiness.Inputs.Person;
using aydoganfbank.web.api2.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api2.Controllers.Test
{
    [Route("api/test/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        #region IoC
        private readonly IServiceContext serviceContext;
        private readonly AydoganFBank.Service.Services.IServiceContext newServiceContext;

        public ValuesController(IServiceContext serviceContext, AydoganFBank.Service.Services.IServiceContext newServiceContext)
        {
            this.serviceContext = serviceContext;
            this.newServiceContext = newServiceContext;
        }
        #endregion

        // GET: api/Values
        [HttpGet]
        public List<string> Get()
        {
            return new List<string>() { "value1", "value2" };
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
