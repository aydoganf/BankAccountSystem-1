using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aydoganfbank.web.api.bussiness.Inputs.Company;
using aydoganfbank.web.api.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service;
using Microsoft.AspNetCore.Http;
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
        public ActionResult<ApiResponse<ICompanyInfo>> CreateCompany([FromBody] CreateCompanyMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManager.CreateCompany(
                        message.CompanyName,
                        message.ResponsablePersonId,
                        message.Address,
                        message.PhoneNumber,
                        message.TaxNumber,
                        message.AccountId));
        }

        [HttpPut("/{id}/change-company-address")]
        public ActionResult<ApiResponse<ICompanyInfo>> ChangeCompanyAddress(int id, [FromBody] ChangeCompanyAddressMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManager.ChangeCompanyAddress(
                        id,
                        message.Address));
        }

        [HttpPut("/{id}/change-company-phone-number")]
        public ActionResult<ApiResponse<ICompanyInfo>> ChangeCompanyPhoneNumber(int id, [FromBody] ChangeCompanyPhoneNumberMessage message)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManager.ChangeCompanyPhoneNumber(
                        id,
                        message.PhoneNumber));
        }


    }
}