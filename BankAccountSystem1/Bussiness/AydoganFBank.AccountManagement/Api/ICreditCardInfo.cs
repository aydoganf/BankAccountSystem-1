using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement.Api
{
    public interface ICreditCardInfo
    {
         int Id { get;  }
         string CreditCardNumber { get;  }
         decimal Limit { get;  }
         int ExtreDay { get;  }
         decimal Debt { get;  }
         int Type { get;  }
         string ValidMonth { get;  }
         string ValidYear { get;  }
         string SecurityCode { get;  }
         bool IsInternetUsageOpen { get;  }
         ICreditCardOwner CreditCardOwner { get;  }

        string CreditCardMaskedNumber { get; }
        decimal UsableLimit { get; }
        DateTime UntilValidDate { get; }
    }
}
