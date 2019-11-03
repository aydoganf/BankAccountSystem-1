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
            public PersonAlreadyExistWithTheGivenIdentityNumberException(string message) 
                : base(1, string.Format("Person already exist with the given identity number. {0}", message))
            {
            }
        }

        public class DepositAmountCanNotBeZeroOrNegativeException : AccountManagementException
        {
            public DepositAmountCanNotBeZeroOrNegativeException(string message) 
                : base(2, string.Format("Deposit amount cannot be zero or negative. {0}", message))
            {
            }
        }

        public class WithdrawAmountCanNotBeZeroOrNegativeException : AccountManagementException
        {
            public WithdrawAmountCanNotBeZeroOrNegativeException(string message) 
                : base(3, string.Format("withdraw amount cannot be zero or negative. {0}", message))
            {
            }
        }

        public class AccountHasNotEnoughBalanceForWithdrawAmount : AccountManagementException
        {
            public AccountHasNotEnoughBalanceForWithdrawAmount(string message) 
                : base(4, string.Format("Account has not enough balance for withdraw amount. {0}", message))
            {
            }
        }

        public class CompanyAlreadyExistWithTheGivenTaxNumberException : AccountManagementException
        {
            public CompanyAlreadyExistWithTheGivenTaxNumberException(string message) 
                : base(5, string.Format("Company already exist with the given tax number. {0}", message))
            {
            }
        }

        public class CreditCardLimitCouldNotBeZeroOrNegative : AccountManagementException
        {
            public CreditCardLimitCouldNotBeZeroOrNegative(string message) 
                : base(6, string.Format("Credit card limit cannot be zero or negative. {0}", message))
            {
            }
        }

        public class CreditCardExtreDayCouldNotZeroOrNegative : AccountManagementException
        {
            public CreditCardExtreDayCouldNotZeroOrNegative(string message) 
                : base(7, string.Format("Credit card extre day cannot be zero or negative. {0}", message))
            {
            }
        }

        public class CreditCardHasNotEnoughLimit : AccountManagementException
        {
            public CreditCardHasNotEnoughLimit(string message) 
                : base(8, string.Format("Credit card has not enough limit. {0}", message))
            {
            }
        }

        public class DifferentAccountTypesCouldNotTransferAssetsToEachOther : AccountManagementException
        {
            public DifferentAccountTypesCouldNotTransferAssetsToEachOther(string message) 
                : base(9, string.Format("Different account types cannot transfer assets to each other. {0}", message))
            {
            }
        }

        public class TransactionOrderCouldNotHasDifferentAccountTypes : AccountManagementException
        {
            public TransactionOrderCouldNotHasDifferentAccountTypes(string message) 
                : base(10, string.Format("Transaction order cannot has different account types. {0}", message))
            {
            }
        }

        public class CreditCardValidDateHasExpired : AccountManagementException
        {
            public CreditCardValidDateHasExpired(string message) 
                : base(11, string.Format("Credit card valid date has expired. {0}", message))
            {
            }
        }

        public class CreditCardOwnerHasAlreadyCreditCard : AccountManagementException
        {
            public CreditCardOwnerHasAlreadyCreditCard(string message) 
                : base(12, string.Format("Credit card owner already has credit card. {0}", message))
            {
            }
        }

        public class AccountOwnerHasAlreadyAnAccountWithGivenAccountType : AccountManagementException
        {
            public AccountOwnerHasAlreadyAnAccountWithGivenAccountType(string message) 
                : base(13, string.Format("Account owner has already an account with given account type. {0}", message))
            {
            }
        }

        public class PersonCouldNotFoundWithGivenEmailAndPassword : AccountManagementException
        {
            public PersonCouldNotFoundWithGivenEmailAndPassword(string message) 
                : base(14, string.Format("Person could not found with given email and password. {0}", message))
            {
            }
        }

        public class PersonCouldNotFoundWithGivenIdentityNumberAndPassword : AccountManagementException
        {
            public PersonCouldNotFoundWithGivenIdentityNumberAndPassword(string message) 
                : base(15, string.Format("Person could not found with given identity number and password. {0}", message))
            {
            }
        }

        public class PersonCouldNotFoundWithGivenIdentityNumber : AccountManagementException
        {
            public PersonCouldNotFoundWithGivenIdentityNumber(string message)
                :base(16, string.Format("Person could not found with given identity number. {0}", message))
            {
            }
        }

        public class AccountCouldNotFoundWithGivenAccountNumber : AccountManagementException
        {
            public AccountCouldNotFoundWithGivenAccountNumber(string message)
                : base(17, string.Format("Account could not found with given account number. {0}", message))
            {

            }
        }

        public class LoginInformationIsNotValid : AccountManagementException
        {
            public LoginInformationIsNotValid(string message) 
                : base(18, string.Format("Login information is not valid. {0}", message))
            {
            }
        }

        public class TokenCouldNotFoundWithGivenInformations : AccountManagementException
        {
            public TokenCouldNotFoundWithGivenInformations(string message) 
                : base(19, string.Format("Token could not found with given informations. {0}", message))
            {
            }
        }

        public class TokenIsNotValid : AccountManagementException
        {
            public TokenIsNotValid(string message) 
                : base(20, string.Format("Token is not valid. {0}", message))
            {
            }
        }
    }
}
