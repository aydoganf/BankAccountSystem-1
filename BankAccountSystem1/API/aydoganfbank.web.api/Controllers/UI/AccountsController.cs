using aydoganfbank.web.api.Utility;
using AydoganFBank.Service.Dispatcher.Context;
using AydoganFBank.Service.Dispatcher.Data;
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
