using System;

namespace AydoganFBank.TransactionManagement.Api
{
    internal static class ApiExtensions
    {

        public static int ToInt(this TransactionOwnerType transactionOwnerType)
        {
            return Convert.ToInt32(transactionOwnerType);
        }

        public static int ToInt(this TransactionStatusEnum transactionStatus)
        {
            return Convert.ToInt32(transactionStatus);
        }

        public static int ToInt(this TransactionDirection transactionDirection)
        {
            return Convert.ToInt32(transactionDirection);
        }

        public static string ToFormattedString(this DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy HH:mm:ss");
        }
    }

    internal static partial class ApiUtils
    {
        public static string GenerateTransactionDescription(
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
