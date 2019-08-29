using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aydoganfbank.web.api.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api.Controllers.Business
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
        public ActionResult<ApiResponse<ICreditCardInfo>> CreateCreditCard([FromBody] CreateCreditCardMessage message)
        {
            switch (message.CreditCardOwner.OwnerType.ParseToEnum<CreditCardOwnerType>())
            {
                case CreditCardOwnerType.Account:
                    return this.HandleResult(
                        () =>
                            serviceContext.CreditCardManager.CreateAccountCreditCard(
                                message.Limit,
                                message.ExtreDate,
                                message.Type,
                                message.ValidMonth,
                                message.ValidYear,
                                message.SecurityCode,
                                message.IsInternetUsageOpen,
                                message.OwnerId));
                case CreditCardOwnerType.Company:
                    return this.HandleResult(
                        () =>
                            serviceContext.CreditCardManager.CreateCompanyCreditCard(
                                message.Limit,
                                message.ExtreDate,
                                message.Type,
                                message.ValidMonth,
                                message.ValidYear,
                                message.SecurityCode,
                                message.IsInternetUsageOpen,
                                message.OwnerId));
                default:
                    return BadRequest();
            }
        }

        [HttpPost("/{id}/do-payment")]
        public ActionResult<ApiResponse<ICreditCardInfo>> DoPayment(int id, [FromBody] DoCreditCardPaymentMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CreditCardManager.DoCreditCardPayment(
                        id,
                        message.Amount,
                        message.InstalmentCount,
                        message.ToAccountId));
        }
    }
}