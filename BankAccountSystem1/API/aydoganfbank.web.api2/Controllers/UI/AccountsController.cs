using aydoganfbank.web.api2.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service.Message.Data;
using AydoganFBank.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api2.Controllers.UI
{
    [Route("api/ui/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        #region IoC
        private readonly IServiceContext serviceContext;

        public AccountsController(IServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }
        #endregion

        [HttpGet(ApiURLActions.UI.AccountsController.GET_ACCOUNT_BY_ID)]
        public ActionResult<ApiResponse<AccountInfo>> GetAccountById(int id)
        {
            return this.HandleResult(
                () =>
                    serviceContext.AccountManagerService.GetAccountInfo(id));
        }

        [HttpGet()]
        public ActionResult<ApiResponse<AccountInfo>> GetAccountByAccountNumber([FromQuery] string accountNumber)
        {
            return this.HandleResult(
                () =>
                    serviceContext.AccountManagerService.GetAccountInfoByAccountNumber(accountNumber));
        }

        [HttpGet(ApiURLActions.UI.AccountsController.GET_ACCOUNT_TYPE_BY_ID)]
        public ActionResult<ApiResponse<AccountTypeInfo>> GetAccountTypeById(int id)
        {
            return this.HandleResult(
                () =>
                    serviceContext.AccountManagerService.GetAccountTypeInfo(id));
        }

        [HttpGet(ApiURLActions.UI.AccountsController.GET_ACCOUNT_TYPE_BY_KEY)]
        public ActionResult<ApiResponse<AccountTypeInfo>> GetAccountTypeByKey([FromQuery] string key)
        {
            return this.HandleResult(
                () =>
                    serviceContext.AccountManagerService.GetAccountTypeByKey(key));
        }
    }
}