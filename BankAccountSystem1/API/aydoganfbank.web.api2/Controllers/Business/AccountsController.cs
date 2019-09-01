using aydoganfbank.web.api.bussiness.Inputs.Account;
using aydoganfbank.web.api2.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service.Message.Data;
using AydoganFBank.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api2.Controllers.Business
{
    [Route("api/business/[controller]")]
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


        [HttpPost(ApiURLActions.Business.AccountsController.CREATE_PERSON_ACCOUNT)]
        public ActionResult<ApiResponse<AccountInfo>> CreatePersonAccount(int personId, [FromBody] CreatePersonAccountMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.AccountManagerService.CreatePersonAccount(
                        message.AccountTypeKey,
                        personId));
        }

        [HttpPost(ApiURLActions.Business.AccountsController.CREATE_COMPANY_ACCOUNT)]
        public ActionResult<ApiResponse<AccountInfo>> CreateCompanyAccount(int companyId, [FromBody] CreateCompanyAccountMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.AccountManagerService.CreateCompanyAccount(
                        message.AccountTypeKey,
                        companyId));
        }

        [HttpPost(ApiURLActions.Business.AccountsController.TRANSFER_ASSETS)]
        public ActionResult<ApiResponse<object>> TransferAssets([FromBody] TransferAssetsMessage message)
        {
            TransactionTypeEnum transactionType = message.TransactionType.ParseToEnum<TransactionTypeEnum>();

            return this.HandleResult(
                () =>
                    serviceContext.AccountManagerService.TransferAssets(
                        message.FromAccountId,
                        message.ToAccountId,
                        message.Amount,
                        transactionType));
        }


        [HttpPut(ApiURLActions.Business.AccountsController.WITHDRAW_ASSETS)]
        public ActionResult<ApiResponse<AccountInfo>> WithdrawMoneyFromOwn(int accountId, [FromBody] WithdrawMoneyFromOwnMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.AccountManagerService.WithdrawMoneyFromOwn(
                        accountId,
                        message.Amount));
        }

        [HttpPut(ApiURLActions.Business.AccountsController.DEPOSIT_ASSETS)]
        public ActionResult<ApiResponse<AccountInfo>> DepositToOwnAccount(int accountId, [FromBody] DepositToOwnAccountMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.AccountManagerService.DepositToOwnAccount(
                        accountId,
                        message.Amount));
        }
    }
}