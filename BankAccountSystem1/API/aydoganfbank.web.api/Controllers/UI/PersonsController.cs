using aydoganfbank.web.api.Utility;
using AydoganFBank.Service.Dispatcher.Context;
using AydoganFBank.Service.Dispatcher.Data;
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