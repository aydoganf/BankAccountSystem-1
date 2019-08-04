using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Common.Exception
{
    public class ServiceException : System.Exception
    {
        public int ExceptionCode { get; set; }
        public string ExceptionMessage { get; set; }

        public ServiceException(int exceptionCode, string message)
        {
            ExceptionCode = exceptionCode;
            ExceptionMessage = message;
        }
    }
}
