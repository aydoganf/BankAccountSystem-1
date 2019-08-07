using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement
{
    public class AccountManagementException : Common.Exception.ServiceException
    {
        private static int ExceptionBlockInit = 20000;

        public AccountManagementException(int exceptionCode, string message) : base(ExceptionBlockInit + exceptionCode, message)
        {
        }

        public class DepositAmountCanNotBeZeroOrNegativeException : AccountManagementException
        {
            public DepositAmountCanNotBeZeroOrNegativeException(string message)
                : base(1, string.Format("Deposit amount can not be zero or negative: {0}", message))
            {
            }
        }

        public class WithdrawAmountCanNotBeZeroOrNegativeException : AccountManagementException
        {
            public WithdrawAmountCanNotBeZeroOrNegativeException(string message)
                : base(2, string.Format("Withdraw amount can not be zero or negative:{0}", message))
            {
            }
        }

        public class PersonAlreadyExistWithTheGivenIdentityNumberException : AccountManagementException
        {
            public PersonAlreadyExistWithTheGivenIdentityNumberException(string message)
                : base(3, string.Format("Person already exist with the given identity number: {0}", message))
            {
            }
        }

        public class CompanyAlreadyExistWithTheGivenTaxNumberException : AccountManagementException
        {
            public CompanyAlreadyExistWithTheGivenTaxNumberException(string message) 
                : base(4, string.Format("Company already exist with the given tax number: {0}", message))
            {
            }
        }
    }
}
