using aydoganfbank.web.api.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api.Controllers.UI
{
    [Route("api/ui/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IServiceContext serviceContext;

        public AccountsController(IServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }

        // GET: api/Accounts/5
        [HttpGet(ApiURLActions.UI.AccountsController.GET_ACCOUNT_BY_ID)]
        public ActionResult<ApiResponse<IAccountInfo>> GetAccountById(int id)
        {
            return this.HandleResult(
                () => 
                    serviceContext.AccountManager.GetAccountInfo(id));
        }

        [HttpGet(ApiURLActions.UI.AccountsController.GET_ACCOUNT_BY_ACCOUNT_NUMBER)]
        public ActionResult<ApiResponse<IAccountInfo>> GetAccountByAccountNumber([FromQuery] string accountNumber)
        {
            return this.HandleResult(
                () => 
                    serviceContext.AccountManager.GetAccountInfoByAccountNumber(accountNumber));
        }

        [HttpGet(ApiURLActions.UI.AccountsController.GET_ACCOUNT_TYPE_BY_ID)]
        public ActionResult<ApiResponse<IAccountTypeInfo>> GetAccountTypeById(int id)
        {
            return this.HandleResult(
                () => 
                    serviceContext.AccountManager.GetAccountTypeInfo(id));
        }

        [HttpGet(ApiURLActions.UI.AccountsController.GET_ACCOUNT_TYPE_BY_KEY)]
        public ActionResult<ApiResponse<IAccountTypeInfo>> GetAccountTypeByKey([FromQuery] string key)
        {
            return this.HandleResult(
                () => 
                    serviceContext.AccountManager.GetAccountTypeByKey(key));
        }
    }
}
