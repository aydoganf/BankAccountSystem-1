using aydoganfbank.web.api2.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AydoganFBank.Service.Dispatcher.Context;
using AydoganFBank.Service.Dispatcher.Data;

namespace aydoganfbank.web.api2.Controllers.UI
{
    [Route("api/ui/[controller]")]
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

        [HttpGet(ApiURLActions.UI.CompaniesController.GET_COMPANY_BY_ID)]
        public ActionResult<ApiResponse<CompanyInfo>> GetCompanyById(int id)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManagerService.GetCompanyInfo(id));
        }

        [HttpGet()]
        public ActionResult<ApiResponse<CompanyInfo>> GetCompanyByResponsablePerson([FromQuery] int responsableId)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManagerService.GetCompanyByResponsableId(responsableId));
        }

        [HttpGet()]
        public ActionResult<ApiResponse<CompanyInfo>> GetCompanyByResponsablePerson([FromQuery] string responsableIdentityNumber)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManagerService.GetCompanyByResponsableIdentityNumber(responsableIdentityNumber));
        }

        [HttpGet()]
        public ActionResult<ApiResponse<CompanyInfo>> GetCompanyByTaxNumber([FromQuery] string taxNumber)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManagerService.GetCompanyByTaxNumber(taxNumber));
        }

        [HttpGet("{companyId}/accounts")]
        public ActionResult<ApiResponse<List<AccountInfo>>> GetCompanyAccounts(int companyId)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManagerService.GetCompanyAccounts(companyId));
        }
    }
}