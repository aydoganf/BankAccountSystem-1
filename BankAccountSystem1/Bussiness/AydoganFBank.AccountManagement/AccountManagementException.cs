using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.AccountManagement
{
    public class AccountManagementException : Context.Exception.ServiceException
    {
        private const int EXCEPTION_CODE_BLOCK = 20000;
        public AccountManagementException(int exceptionCode, string message) : base(EXCEPTION_CODE_BLOCK + exceptionCode, message)
        {
        }

        public class PersonAlreadyExistWithTheGivenIdentityNumberException : AccountManagementException
        {
            public PersonAlreadyExistWithTheGivenIdentityNumberException(string message) : base(1, message)
            {
            }
        }

        public class DepositAmountCanNotBeZeroOrNegativeException : AccountManagementException
        {
            public DepositAmountCanNotBeZeroOrNegativeException(string message) : base(2, message)
            {
            }
        }

        public class WithdrawAmountCanNotBeZeroOrNegativeException : AccountManagementException
        {
            public WithdrawAmountCanNotBeZeroOrNegativeException(string message) : base(3, message)
            {
            }
        }

        public class AccountHasNotEnoughBalanceForWithdrawAmount : AccountManagementException
        {
            public AccountHasNotEnoughBalanceForWithdrawAmount(string message) : base(4, message)
            {
            }
        }

        public class CompanyAlreadyExistWithTheGivenTaxNumberException : AccountManagementException
        {
            public CompanyAlreadyExistWithTheGivenTaxNumberException(string message) : base(5, message)
            {
            }
        }

        public class CreditCardLimitCouldNotBeZeroOrNegative : AccountManagementException
        {
            public CreditCardLimitCouldNotBeZeroOrNegative(string message) : base(6, message)
            {
            }
        }

        public class CreditCardExtreDayCouldNotZeroOrNegative : AccountManagementException
        {
            public CreditCardExtreDayCouldNotZeroOrNegative(string message) : base(7, message)
            {
            }
        }

        public class CreditCardHasNotEnoughLimit : AccountManagementException
        {
            public CreditCardHasNotEnoughLimit(string message) : base(8, message)
            {
            }
        }

        public class DifferentAccountTypesCouldNotTransferAssetsToEachOther : AccountManagementException
        {
            public DifferentAccountTypesCouldNotTransferAssetsToEachOther(string message) : base(9, message)
            {
            }
        }

        public class TransactionOrderCouldNotHasDifferentAccountTypes : AccountManagementException
        {
            public TransactionOrderCouldNotHasDifferentAccountTypes(string message) : base(10, message)
            {
            }
        }
    }
}
