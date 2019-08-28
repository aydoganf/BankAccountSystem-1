using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aydoganfbank.web.api.Utility
{
    public static class ApiURLActions
    {
        public static class Business
        {
            public static class AccountsController
            {
                public const string CREATE_PERSON_ACCOUNT = "/person/{personId}/create-account";
                public const string CREATE_COMPANY_ACCOUNT = "/company/{companyId}/create-account";
                public const string TRANSFER_ASSETS = "/transaction/transfer-assets";
                public const string WITHDRAW_ASSETS = "/{accountId}/transaction/withdraw-assets";
                public const string DEPOSIT_ASSETS = "/{accountId}/transaction/deposit-assets";
            }

            public static class PersonsController
            {
                public const string CHANGE_LAST_NAME = "/{personId}/change-last-name";
                public const string CHANGE_EMAIL = "/{personId}/change-email";
            }
        }
    }
}
