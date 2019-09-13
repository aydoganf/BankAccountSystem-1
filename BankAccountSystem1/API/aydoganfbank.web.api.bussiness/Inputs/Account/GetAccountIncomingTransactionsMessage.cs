using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aydoganfbank.web.api.bussiness.Inputs.Account
{
    public class GetAccountIncomingTransactionsMessage
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
