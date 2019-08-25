using aydoganfbank.web.api.bussiness.Inputs.Account;
using aydoganfbank.web.api.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api.Controllers.Bussiness
{
    [Route("api/business/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IServiceContext serviceContext;

        public AccountsController(IServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }


        [HttpPost("/person/{accountTypeKey}/{personId}")]
        public ActionResult<ApiResponse<IAccountInfo>> CreatePersonAccount(string accountTypeKey, int personId)
        {
            return this.HandleResult(() => serviceContext.AccountManager.CreatePersonAccount(accountTypeKey, personId));
        }

        [HttpPost("/company/{accountTypeKey}/{companyId}")]
        public ActionResult<ApiResponse<IAccountInfo>> CreateCompanyAccount(string accountTypeKey, int companyId)
        {
            return this.HandleResult(() => serviceContext.AccountManager.CreateCompanyAccount(accountTypeKey, companyId));
        }

        [HttpPost("/transferAssets")]
        public ActionResult<ApiResponse<object>> TransferAssets([FromBody] TransferAssetsMessage message)
        {
            TransactionTypeEnum transactionType = message.TransactionType.ParseToEnum<TransactionTypeEnum>();

            return this.HandleResult(() => serviceContext.AccountManager
                .TransferAssets(message.FromAccountId, message.ToAccountId, message.Amount, transactionType));
        }


        [HttpPut("withdraw/{accountId}")]
        public ActionResult<ApiResponse<IAccountInfo>> WithdrawMoneyFromOwn(int accountId, [FromBody] WithdrawMoneyFromOwnMessage message)
        {
            return this.HandleResult(() => serviceContext.AccountManager.WithdrawMoneyFromOwn(accountId, message.Amount));
        }

        [HttpPut("deposit/{accountId}")]
        public ActionResult<ApiResponse<IAccountInfo>> DepositToOwnAccount(int accountId, [FromBody] DepositToOwnAccountMessage message)
        {
            return this.HandleResult(() => serviceContext.AccountManager.DepositToOwnAccount(accountId, message.Amount));
        }

        
    }
}
