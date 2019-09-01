using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aydoganfbank.web.api2.Utility
{
    public class ApiResponse<T>
    {
        public T Result { get; set; }
        public int ResultCode { get; set; }
        public string ResultMessage { get; set; }
    }
}
