using aydoganfbank.web.api.Utility;
using AydoganFBank.Service.Dispatcher.Context;
using AydoganFBank.Service.Dispatcher.Data;
using Microsoft.AspNetCore.Mvc;

namespace aydoganfbank.web.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IServiceContext serviceContext;

        public ValuesController(IServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<ApiResponse<PersonInfo>> GetPersonById()
        {
            return this.HandleResult(() => serviceContext.PersonManagerService.GetPersonInfo(1));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
