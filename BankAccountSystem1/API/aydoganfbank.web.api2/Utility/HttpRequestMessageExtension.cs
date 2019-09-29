using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace aydoganfbank.web.api2.Utility
{
    public static class HttpRequestMessageExtension
    {
        public static string GetAuthenticationHeader(this HttpRequest request)
        {
            IEnumerable<string> headers;
            request.Headers.TryGetValues("X-Auth", out headers);
            if (headers == null || headers.Count() == 0)
                return string.Empty;
            return headers.First();
        }
    }
}
