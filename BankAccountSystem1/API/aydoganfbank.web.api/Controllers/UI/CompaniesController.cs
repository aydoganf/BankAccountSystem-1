using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aydoganfbank.web.api.Utility;
using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api.Controllers.UI
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
        public ActionResult<ApiResponse<ICompanyInfo>> GetCompanyById(int id)
        {
            return this.HandleResult(
                () => 
                    serviceContext.CompanyManager.GetCompanyInfo(id));
        }

        [HttpGet()]
        public ActionResult<ApiResponse<ICompanyInfo>> GetCompanyByResponsablePerson([FromQuery] int responsableId)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManager.GetCompanyByResponsableId(responsableId));
        }

        [HttpGet()]
        public ActionResult<ApiResponse<ICompanyInfo>> GetCompanyByResponsablePerson([FromQuery] string responsableIdentityNumber)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManager.GetCompanyByResponsableIdentityNumber(responsableIdentityNumber));
        }

        [HttpGet()]
        public ActionResult<ApiResponse<ICompanyInfo>> GetCompanyByTaxNumber([FromQuery] string taxNumber)
        {
            return this.HandleResult(
                () =>
                    serviceContext.CompanyManager.GetCompanyByTaxNumber(taxNumber));
        }
    }
}