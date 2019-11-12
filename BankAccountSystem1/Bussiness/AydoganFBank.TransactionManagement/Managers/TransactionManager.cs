using AydoganFBank.Context.IoC;
using AydoganFBank.TransactionManagement.Api;
using AydoganFBank.TransactionManagement.Domain;
using AydoganFBank.TransactionManagement.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.TransactionManagement.Managers
{
    public class TransactionManager : ITransactionManager
    {
        #region IoC
        private readonly ICoreContext coreContext;

        public TransactionManager(ICoreContext coreContext)
        {
            this.coreContext = coreContext;
        }
        #endregion

        internal ITransactionInfo MakeTransaction(
            ITransactionOwner from,
            ITransactionOwner to,
            decimal amount,
            TransactionTypeEnum transactionType,
            ITransactionOwner owner)
        {
            var transaction = coreContext.New<AccountTransactionDomainEntity>().
                With(from, to, amount, transactionType, TransactionStatusEnum.InProgress, owner);

            transaction.Insert();

            return transaction;
        }

        internal ITransactionInfo SetTransactionStatus(ITransactionInfo transaction, TransactionStatusEnum status)
        {
            transaction.SetStatus(status);
            return transaction;
        }

        internal ITransactionDetailInfo CreateTransactionDetail(ITransactionInfo transaction, TransactionDirection transactionDirection)
        {
            string description = ApiUtils.GenerateTransactionDescription(
                transactionDirection, transaction.FromTransactionOwner, transaction.ToTransactionOwner, transaction.Amount);

            if (transactionDirection == TransactionDirection.In)
            {
                return coreContext.New<TransactionDetailDomainEntity>()
                    .With(description, transaction.TransactionDate, transaction, (ITransactionDetailOwner)transaction.ToTransactionOwner, transactionDirection);
            }
            else
            {
                return coreContext.New<TransactionDetailDomainEntity>()
                    .With(description, transaction.TransactionDate, transaction, (ITransactionDetailOwner)transaction.FromTransactionOwner, transactionDirection);
            }
        }

        ITransactionDetailInfo ITransactionManager.CreateTransactionDetail(ITransactionInfo transaction, TransactionDirection transactionDirection)
            => CreateTransactionDetail(transaction, transactionDirection);

        ITransactionInfo ITransactionManager.MakeTransaction(
            ITransactionOwner from,
            ITransactionOwner to,
            decimal amount,
            TransactionTypeEnum transactionType,
            ITransactionOwner owner)
            => MakeTransaction(from, to, amount, transactionType, owner);

        ITransactionInfo ITransactionManager.SetTransactionStatus(ITransactionInfo transaction, TransactionStatusEnum status)
            => SetTransactionStatus(transaction, status);
    }
}
