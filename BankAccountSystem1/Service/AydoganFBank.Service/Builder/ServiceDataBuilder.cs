using AydoganFBank.AccountManagement.Api;
using AydoganFBank.Service.Message.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Service.Builder
{
    public class ServiceDataBuilder
    {
        public AccountInfo BuildAccountInfo(IAccountInfo account)
        {
            return new AccountInfo()
            {
                AccountNumber = account.AccountNumber,
                AccountOwner = BuildAccountOwner(account.AccountOwner),
                AccountType = BuildAccountTypeInfo(account.AccountType),
                Balance = account.Balance,
                Id = account.Id
            };
        }

        public List<AccountInfo> BuildAccountInfoList(List<IAccountInfo> accountInfos)
        {
            if (accountInfos == null || accountInfos.Count == 0)
                return null;

            List<AccountInfo> accounts = new List<AccountInfo>();
            foreach (var accountInfo in accountInfos)
            {
                accounts.Add(BuildAccountInfo(accountInfo));
            }
            return accounts;
        }

        public AccountTypeInfo BuildAccountTypeInfo(IAccountTypeInfo accountType)
        {
            return new AccountTypeInfo()
            {
                AssetsUnit = accountType.AssetsUnit,
                Id = accountType.Id,
                TypeKey = accountType.TypeKey,
                TypeName = accountType.TypeName
            };
        }

        public AccountOwner BuildAccountOwner(IAccountOwner accountOwner)
        {
            return new AccountOwner()
            {
                DisplayName = accountOwner.DisplayName,
                OwnerId = accountOwner.OwnerId,
                OwnerType = accountOwner.OwnerType
            };
        }

        public PersonInfo BuildPersonInfo(IPersonInfo person)
        {
            return new PersonInfo()
            {
                EmailAddress = person.EmailAddress,
                FirstName = person.FirstName,
                FullName = person.FullName,
                Id = person.Id,
                IdentityNumber = person.IdentityNumber,
                LastName = person.LastName
            };
        }

        public List<PersonInfo> BuildPersonInfoList(List<IPersonInfo> personInfos)
        {
            List<PersonInfo> people = new List<PersonInfo>();
            foreach (var personInfo in personInfos)
            {
                people.Add(BuildPersonInfo(personInfo));
            }
            return people;
        }

        public CompanyInfo BuildCompanyInfo(ICompanyInfo company)
        {
            return new CompanyInfo()
            {
                Address = company.Address,
                CompanyName = company.CompanyName,
                Id = company.Id,
                PhoneNumber = company.PhoneNumber,
                ResponsablePerson = BuildPersonInfo(company.ResponsablePerson),
                TaxNumber = company.TaxNumber
            };
        }

        public CreditCardInfo BuildCreditCardInfo(ICreditCardInfo creditCard)
        {
            return new CreditCardInfo()
            {
                CreditCardMaskedNumber = creditCard.CreditCardMaskedNumber,
                CreditCardNumber = creditCard.CreditCardNumber,
                CreditCardOwner = BuildCreditCardOwner(creditCard.CreditCardOwner),
                Debt = creditCard.Debt,
                ExtreDay = creditCard.ExtreDay,
                Id = creditCard.Id,
                IsInternetUsageOpen = creditCard.IsInternetUsageOpen,
                Limit = creditCard.Limit,
                SecurityCode = creditCard.SecurityCode,
                Type = creditCard.Type,
                UntilValidDate = creditCard.UntilValidDate,
                UsableLimit = creditCard.UsableLimit,
                ValidMonth = creditCard.ValidMonth,
                ValidYear = creditCard.ValidYear
            };
        }

        public CreditCardOwner BuildCreditCardOwner(ICreditCardOwner creditCardOwner)
        {
            return new CreditCardOwner()
            {
                AssetsUnit = creditCardOwner.AssetsUnit,
                CreditCardOwnerType = creditCardOwner.CreditCardOwnerType,
                OwnerId = creditCardOwner.OwnerId
            };
        }

        public TransactionStatusInfo BuildTransactionStatusInfo(ITransactionStatusInfo transactionStatusInfo)
        {
            return new TransactionStatusInfo()
            {
                StatusId = transactionStatusInfo.StatusId,
                StatusKey = transactionStatusInfo.StatusKey,
                StatusName = transactionStatusInfo.StatusName
            };
        }

        public TransactionTypeInfo BuildTransactionTypeInfo(ITransactionTypeInfo transactionTypeInfo)
        {
            return new TransactionTypeInfo()
            {
                TypeId = transactionTypeInfo.TypeId,
                TypeKey = transactionTypeInfo.TypeKey,
                TypeName = transactionTypeInfo.TypeName
            };
        }

        public TransactionOwner BuildTransactionOwner(ITransactionOwner transactionOwner)
        {
            return new TransactionOwner()
            {
                AssetsUnit = transactionOwner.AssetsUnit,
                OwnerId = transactionOwner.OwnerId,
                OwnerType = transactionOwner.OwnerType,
                TransactionDetailDisplayName = transactionOwner.TransactionDetailDisplayName
            };
        }

        public TransactionInfo BuildTransactionInfo(ITransactionInfo transactionInfo)
        {
            return new TransactionInfo()
            {
                Amount = transactionInfo.Amount,
                FromTransactionOwner = BuildTransactionOwner(transactionInfo.FromTransactionOwner),
                ToTransactionOwner = BuildTransactionOwner(transactionInfo.ToTransactionOwner),
                TransactionDate = transactionInfo.TransactionDate,
                TransactionOwner = transactionInfo.TransactionOwner != null ? BuildTransactionOwner(transactionInfo.TransactionOwner) : null,
                TransactionStatus = BuildTransactionStatusInfo(transactionInfo.TransactionStatus),
                TransactionType = BuildTransactionTypeInfo(transactionInfo.TransactionType)
            };
        }

        public List<TransactionInfo> BuildTransactionInfoList(List<ITransactionInfo> transactionInfos)
        {
            List<TransactionInfo> transactions = new List<TransactionInfo>();
            foreach (var transactionInfo in transactionInfos)
            {
                transactions.Add(BuildTransactionInfo(transactionInfo));
            }
            return transactions;
        }
    }
}
