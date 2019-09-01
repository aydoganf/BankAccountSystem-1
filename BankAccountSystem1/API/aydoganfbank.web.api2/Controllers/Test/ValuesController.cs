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
        public ActionResult<ApiResponse<IPersonInfo>> Get()
        {
            return this.HandleResult(() => serviceContext.PersonManager.GetPersonInfo(1));
        }

        // GET: api/Values/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<ApiResponse<AydoganFBank.Service.Message.Data.AccountInfo>> Get(int id)
        {
            return this.HandleResult(() => newServiceContext.AccountManagerService.GetAccountInfo(id));
        }

        // POST: api/Values
        [HttpPost]
        public ActionResult<ApiResponse<IPersonInfo>> Post([FromBody] CreatePersonMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.PersonManager.CreatePerson(
                        message.FirstName,
                        message.LastName,
                        message.EmailAddress,
                        message.IdentityNumber));
        }

        // PUT: api/Values/5
        [HttpPut("{id}/change-last-name")]
        public ActionResult<ApiResponse<IPersonInfo>> Put(int id, [FromBody] ChangePersonLastNameMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.PersonManager.ChangePersonLastName(
                        id,
                        message.LastName));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
