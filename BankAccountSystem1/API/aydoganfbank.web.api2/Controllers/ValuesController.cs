using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aydoganfbank.web.api2.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        #region IoC
        private readonly IServiceContext serviceContext;

        public ValuesController(IServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }
        #endregion

        // GET: api/Values
        [HttpGet]
        public ActionResult<ApiResponse<IPersonInfo>> Get()
        {
            return this.HandleResult(() => serviceContext.PersonManager.GetPersonInfo(1));
        }

        // GET: api/Values/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
