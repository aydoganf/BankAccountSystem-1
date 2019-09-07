using aydoganfbank.web.api.bussiness.Inputs.CreditCard;
using aydoganfbank.web.api2.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service.Message.Data;
using AydoganFBank.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api2.Controllers.Business
{
    [Route("api/business/[controller]")]
    [ApiController]
    public class CreditCardsController : ControllerBase
    {
        #region IoC
        private readonly IServiceContext serviceContext;

        public CreditCardsController(IServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }
        #endregion

        [HttpPost]
        public ActionResult<ApiResponse<CreditCardInfo>> CreateCreditCard([FromBody] CreateCreditCardMessage message)
        {
            switch (message.CreditCardOwner.OwnerType.ParseToEnum<CreditCardOwnerType>())
            {
                case CreditCardOwnerType.Account:
                    return this.HandleResult(
                        () =>
                            serviceContext.CreditCardManagerService.CreateAccountCreditCard(
                                message.Limit,
                                message.ExtreDay,
                                message.Type,
                                message.ValidMonth,
                                message.ValidYear,
                                message.SecurityCode,
                                message.IsInternetUsageOpen,
                                message.CreditCardOwner.OwnerId));
                default:
                    return BadRequest();
            }
        }

        [HttpPost(ApiURLActions.Business.CreditCardsController.DO_CREDIT_CARD_PAYMENT)]
        public ActionResult<ApiResponse<CreditCardInfo>> DoPayment(int id, [FromBody] DoCreditCardPaymentMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CreditCardManagerService.DoCreditCardPayment(
                        id,
                        message.Amount,
                        message.InstalmentCount,
                        message.ToAccountId));
        }

        [HttpPost(ApiURLActions.Business.CreditCardsController.DO_CREDIT_CARD_PAYMENT_WITH_SECURITY_INFOS)]
        public ActionResult<ApiResponse<CreditCardInfo>> DoPaymentWithSecurityInfos([FromBody] DoPaymentWithSecurityInfosMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CreditCardManagerService.DoCreditCardPayment(
                        message.CreditCardNumber,
                        message.ValidMonth,
                        message.ValidYear,
                        message.SecurityCode,
                        message.Amount,
                        message.InstalmentCount,
                        message.ToAccountId));
        }
    }
}