using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace aydoganfbank.web.api.Middlewares
{
    public class CamelCaseContractResolver : CamelCasePropertyNamesContractResolver
    {
    }
}
