using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aydoganfbank.web.api.bussiness.Inputs.Person
{
    public class CreatePersonMessage
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string IdentityNumber { get; set; }
    }
}
