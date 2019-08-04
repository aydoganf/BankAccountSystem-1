using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.Common;
using AydoganFBank.Common.IoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Repository
{
    internal class RepositoryFactory
    {
        private readonly ICoreContext coreContext;

        public RepositoryFactory(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }

        private T GetRepositoryInstance<T>()
        {
            return coreContext.New<T>();
        }

        public void Get(RepositoryType repositoryType)
        {
            switch (repositoryType)
            {
                case RepositoryType.Account:
                    GetRepositoryInstance<IAccountRepository>();
                    break;
                case RepositoryType.AccountTransaction:
                    break;
                case RepositoryType.AccountType:
                    break;
                case RepositoryType.Company:
                    break;
                case RepositoryType.Person:
                    break;
                case RepositoryType.TransactionStatus:
                    break;
                case RepositoryType.TransactionType:
                    break;
                default:
                    break;
            }
        }
    }

    internal enum RepositoryType
    {
        Account,
        AccountTransaction,
        AccountType,
        Company,
        Person,
        TransactionStatus,
        TransactionType
    }
}
