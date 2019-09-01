using aydoganfbank.web.api2.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service.Message.Data;
using AydoganFBank.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api2.Controllers.UI
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

        [HttpGet(ApiURLActions.UI.PersonsController.GET_PERSON_BY_ID)]
        public ActionResult<ApiResponse<PersonInfo>> GetPersonById(int id)
        {
            return this.HandleResult(
                () =>
                    serviceContext.PersonManagerService.GetPersonInfo(id));
        }

        [HttpGet()]
        public ActionResult<ApiResponse<PersonInfo>> GetPersonByIdentityNumber([FromQuery] string identityNumber)
        {
            return this.HandleResult(
                () =>
                    serviceContext.PersonManagerService.GetPersonByIdentityNumber(identityNumber));
        }
    }
}