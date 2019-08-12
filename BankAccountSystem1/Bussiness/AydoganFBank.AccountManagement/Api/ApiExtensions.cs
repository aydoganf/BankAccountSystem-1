using AydoganFBank.AccountManagement.Domain;
using AydoganFBank.AccountManagement.Repository;
using AydoganFBank.Common;
using AydoganFBank.Common.IoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace AydoganFBank.AccountManagement.Api
{
    internal static class ApiExtensions
    {
        public static int ToInt(this AccountOwnerType accountOwnerType)
        {
            return Convert.ToInt32(accountOwnerType);
        }

        public static int ToInt(this TransactionOwnerType transactionOwnerType)
        {
            return Convert.ToInt32(transactionOwnerType);
        }

        public static int ToInt(this TransactionStatusEnum transactionStatus)
        {
            return Convert.ToInt32(transactionStatus);
        }
    }

    internal static partial class ApiUtils
    {
        public static string GenerateTransactionMessage(
            TransactionDirection transactionDirection, 
            ITransactionOwner from, 
            ITransactionOwner to, 
            decimal amount
            )
        {
            string message = string.Empty;
            string date = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            switch (transactionDirection)
            {
                case TransactionDirection.In:
                    message = string.Format("{0} have sent {1}{2} to you on {3}.", 
                        from.TransactionDetailDisplayName, 
                        amount, 
                        to.AssetsUnit, 
                        date);
                    break;
                case TransactionDirection.Out:
                    message = string.Format("You sent {0} to {1}{2} on {3}", 
                        amount,
                        from.AssetsUnit,
                        to.TransactionDetailDisplayName, 
                        date);
                    break;
                default:
                    break;
            }
            return message;
        }
    }
}
