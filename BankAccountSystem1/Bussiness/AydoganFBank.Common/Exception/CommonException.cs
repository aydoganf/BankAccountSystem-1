﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Common.Exception
{
    public class CommonException : ServiceException
    {
        private static int ExceptionBlockInit = 10000;

        public CommonException(int exceptionCode, string message) 
            : base(ExceptionBlockInit + exceptionCode, message)
        {
        }

        public class RequiredParameterMissingException : CommonException
        {
            public RequiredParameterMissingException(string message) : base(1, string.Format("Required parameter is missing: {0}", message))
            {
            }
        }
    }
}