using aydoganfbank.web.api.bussiness.Inputs.Person;
using aydoganfbank.web.api2.Utility;
using Microsoft.AspNetCore.Mvc;
using AydoganFBank.Service.Dispatcher.Context;
using AydoganFBank.Service.Dispatcher.Data;

namespace aydoganfbank.web.api2.Controllers.Business
{
    [Route("api/business/[controller]")]
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

        [HttpPut("")]
        public ActionResult<ApiResponse<PersonInfo>> CreatePerson([FromBody] CreatePersonMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.PersonManagerService.CreatePerson(
                        message.FirstName,
                        message.LastName,
                        message.EmailAddress,
                        message.IdentityNumber));
        }

        [HttpPut(ApiURLActions.Business.PersonsController.CHANGE_LAST_NAME)]
        public ActionResult<ApiResponse<PersonInfo>> ChangePersonLastName(int personId, [FromBody] ChangePersonLastNameMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.PersonManagerService.ChangePersonLastName(
                        personId,
                        message.LastName));
        }

        [HttpPut(ApiURLActions.Business.PersonsController.CHANGE_EMAIL)]
        public ActionResult<ApiResponse<PersonInfo>> ChangePersonEmailAddress(int personId, [FromBody] ChangePersonEmailAddressMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.PersonManagerService.ChangePersonEmailAddress(
                        personId,
                        message.EmailAddress));
        }
    }
}