using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.Context.Exception
{
    public class CommonException : ServiceException
    {
        private static int ExceptionBlockInit = 10000;

        public CommonException(int exceptionCode, string message) 
            : base(ExceptionBlockInit + exceptionCode, message)
        {
        }

        public class AuthenticationRequiredException : CommonException
        {
            public AuthenticationRequiredException()
                : base(0, "Authentication required!")
            {
            }
        }

        public class RequiredParameterMissingException : CommonException
        {
            public RequiredParameterMissingException(string message) 
                : base(1, string.Format("Required parameter is missing: {0}", message))
            {
            }
        }

        public class EntityNotFoundInDbContextException : CommonException
        {
            public EntityNotFoundInDbContextException(string message) : 
                base(2, "Entity not found in database")
            {
            }
        }
    }
}
