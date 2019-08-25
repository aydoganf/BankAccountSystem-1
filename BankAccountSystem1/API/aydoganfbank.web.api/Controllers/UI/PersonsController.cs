using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aydoganfbank.web.api.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api.Controllers.UI
{
    [Route("api/ui/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        #region IoC
        private readonly IServiceContext serviceContext;

        public PersonsController(IServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }
        #endregion

        [HttpGet("/{personId}")]
        public ActionResult<ApiResponse<IPersonInfo>> GetPersonById(int personId)
        {
            return this.HandleResult(() => serviceContext.PersonManager.GetPersonInfo(personId));
        }

        [HttpGet("/identityNumber/{identityNumber}")]
        public ActionResult<ApiResponse<IPersonInfo>> GetPersonByIdentityNumber(string identityNumber)
        {
            return this.HandleResult(() => serviceContext.PersonManager.GetPersonByIdentityNumber(identityNumber));
        }


    }
}