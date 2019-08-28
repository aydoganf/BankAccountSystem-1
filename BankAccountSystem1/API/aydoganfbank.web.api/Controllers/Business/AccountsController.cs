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


        [HttpPost(ApiURLActions.Business.AccountsController.CREATE_PERSON_ACCOUNT)]
        public ActionResult<ApiResponse<IAccountInfo>> CreatePersonAccount(int personId, [FromBody] CreatePersonAccountMessage message)
        {
            return this.HandleResult(
                () => 
                    serviceContext.AccountManager.CreatePersonAccount(
                        message.AccountTypeKey, 
                        personId));
        }

        [HttpPost(ApiURLActions.Business.AccountsController.CREATE_COMPANY_ACCOUNT)]
        public ActionResult<ApiResponse<IAccountInfo>> CreateCompanyAccount(int companyId, [FromBody] CreateCompanyAccountMessage message)
        {
            return this.HandleResult(
                () => 
                    serviceContext.AccountManager.CreateCompanyAccount(
                        message.AccountTypeKey, 
                        companyId));
        }

        [HttpPost(ApiURLActions.Business.AccountsController.TRANSFER_ASSETS)]
        public ActionResult<ApiResponse<object>> TransferAssets([FromBody] TransferAssetsMessage message)
        {
            TransactionTypeEnum transactionType = message.TransactionType.ParseToEnum<TransactionTypeEnum>();

            return this.HandleResult(
                () => 
                    serviceContext.AccountManager.TransferAssets(
                        message.FromAccountId, 
                        message.ToAccountId, 
                        message.Amount, 
                        transactionType));
        }


        [HttpPut(ApiURLActions.Business.AccountsController.WITHDRAW_ASSETS)]
        public ActionResult<ApiResponse<IAccountInfo>> WithdrawMoneyFromOwn(int accountId, [FromBody] WithdrawMoneyFromOwnMessage message)
        {
            return this.HandleResult(
                () => 
                    serviceContext.AccountManager.WithdrawMoneyFromOwn(
                        accountId, 
                        message.Amount));
        }

        [HttpPut(ApiURLActions.Business.AccountsController.DEPOSIT_ASSETS)]
        public ActionResult<ApiResponse<IAccountInfo>> DepositToOwnAccount(int accountId, [FromBody] DepositToOwnAccountMessage message)
        {
            return this.HandleResult(
                () => 
                    serviceContext.AccountManager.DepositToOwnAccount(
                        accountId, 
                        message.Amount));
        }

        
    }
}
