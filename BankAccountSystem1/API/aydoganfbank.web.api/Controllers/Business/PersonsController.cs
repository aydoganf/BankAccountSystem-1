using aydoganfbank.web.api.bussiness.Inputs.Person;
using aydoganfbank.web.api.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api.Controllers.Business
{
    [Route("api/[controller]")]
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
        public ActionResult<ApiResponse<IPersonInfo>> CreatePerson([FromBody] CreatePersonMessage message)
        {
            return this.HandleResult(() => serviceContext.PersonManager.CreatePerson(
                message.FirstName, message.LastName, message.EmailAddress, message.IdentityNumber));
        }

        [HttpPut("/{personId}/changePersonLastName")]
        public ActionResult<ApiResponse<IPersonInfo>> ChangePersonLastName(int personId, [FromBody] ChangePersonLastNameMessage message)
        {
            return this.HandleResult(() => serviceContext.PersonManager.ChangePersonLastName(personId, message.LastName));
        }

        [HttpPut("/{personId}/changePersonEmailAddress")]
        public ActionResult<ApiResponse<IPersonInfo>> ChangePersonEmailAddress(int personId, [FromBody] ChangePersonEmailAddressMessage message)
        {
            return this.HandleResult(() => serviceContext.PersonManager.ChangePersonEmailAddress(personId, message.EmailAddress));
        }
    }
}