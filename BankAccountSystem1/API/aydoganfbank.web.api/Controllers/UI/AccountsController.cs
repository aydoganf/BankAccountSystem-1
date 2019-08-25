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
        [HttpGet("/{id}")]
        public ActionResult<ApiResponse<IAccountInfo>> GetAccountById(int id)
        {
            return this.HandleResult(() => serviceContext.AccountManager.GetAccountInfo(id));
        }

        [HttpGet("/accountNumber/{accountNumber}")]
        public ActionResult<ApiResponse<IAccountInfo>> GetAccountByAccountNumber(string accountNumber)
        {
            return this.HandleResult(() => serviceContext.AccountManager.GetAccountInfoByAccountNumber(accountNumber));
        }

        [HttpGet("accountType/{id}")]
        public ActionResult<ApiResponse<IAccountTypeInfo>> GetAccountTypeById(int id)
        {
            return this.HandleResult(() => serviceContext.AccountManager.GetAccountTypeInfo(id));
        }

        [HttpGet("accountType/key/{key}")]
        public ActionResult<ApiResponse<IAccountTypeInfo>> GetAccountTypeByKey(string key)
        {
            return this.HandleResult(() => serviceContext.AccountManager.GetAccountTypeByKey(key));
        }
    }
}
