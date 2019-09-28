using aydoganfbank.web.api.bussiness.Inputs.Company;
using aydoganfbank.web.api.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service.Dispatcher.Context;
using AydoganFBank.Service.Dispatcher.Data;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api.Controllers.Business
{
    [Route("api/business/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        #region IoC
        private readonly IServiceContext serviceContext;

        public CompaniesController(IServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }
        #endregion

        [HttpPost]
        public ActionResult<ApiResponse<CompanyInfo>> CreateCompany([FromBody] CreateCompanyMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManagerService.CreateCompany(
                        message.CompanyName,
                        message.ResponsablePersonId,
                        message.Address,
                        message.PhoneNumber,
                        message.TaxNumber));
        }

        [HttpPut("/{id}/change-company-address")]
        public ActionResult<ApiResponse<CompanyInfo>> ChangeCompanyAddress(int id, [FromBody] ChangeCompanyAddressMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManagerService.ChangeCompanyAddress(
                        id,
                        message.Address));
        }

        [HttpPut("/{id}/change-company-phone-number")]
        public ActionResult<ApiResponse<CompanyInfo>> ChangeCompanyPhoneNumber(int id, [FromBody] ChangeCompanyPhoneNumberMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManagerService.ChangeCompanyPhoneNumber(
                        id,
                        message.PhoneNumber));
        }


    }
}