using System;

namespace AydoganFBank.TransactionManagement.Api
{
    internal static class ApiExtensions
    {
        public static int ToInt(this TransactionOwnerType transactionOwnerType)
        {
            return Convert.ToInt32(transactionOwnerType);
        }

        public static int ToInt(this TransactionDetailOwnerType transactionDetailOwnerType)
        {
            return Convert.ToInt32(transactionDetailOwnerType);
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

                    if (from == null)
                    {
                        // deposit to own account
                        message = string.Format("You deposit {0} {1} to your account on {2}.", 
                            amount, 
                            to.AssetsUnit, 
                            date);
                    }
                    else
                    {
                        message = string.Format("{0} have sent {1} {2} to you on {3}.",
                            from.TransactionDetailDisplayName,
                            amount,
                            to.AssetsUnit,
                            date);
                    }

                    
                    break;
                case TransactionDirection.Out:

                    if (to == null)
                    {
                        message = string.Format("You withdraw {0} {1} from your account on {2}", 
                            amount, 
                            from.AssetsUnit, 
                            date);
                    }
                    else
                    {
                        message = string.Format("You sent {0} {1} to {2} on {3}",
                            amount,
                            from.AssetsUnit,
                            to.TransactionDetailDisplayName,
                            date);
                    }

                    
                    break;
                default:
                    break;
            }
            return message;
        }
    }
}
