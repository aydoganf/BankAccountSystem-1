using AydoganFBank.Service.Dispatcher.Data;
using System.Collections.Generic;

namespace AccountApp.Models
{
    public class AccountOverview
    {
        public AccountInfo Account { get; set; }
        public CreditCardInfo CreditCard { get; set; }
        public List<TransactionInfo> TransactionList { get; set; }
        public List<TransactionDetailInfo> TransactionDetailList { get; set; }

        public string AssetsUnit => Account.AccountType.AssetsUnit;
        public decimal AccountBalance => Account.Balance;
        public decimal Dept => CreditCard != null ? CreditCard.Debt : 0;
        public decimal TotalAsset => AccountBalance - Dept;
        public decimal DeptBalanceRatio 
        {
            get
            {
                if (TotalAsset <= 0)
                {
                    return 100;
                }
                else
                {
                    return Dept / AccountBalance;
                }
            }
        }

        public string AccountBalanceWithAssetsUnit => $"{AccountBalance} {Account.AccountType.AssetsUnit}";
        public string DeptWithAssetsUnit => $"{Dept} {Account.AccountType.AssetsUnit}";
        public string TotalAssetWithAssetsUnit => $"{TotalAsset} {Account.AccountType.AssetsUnit}";

        public AccountOverview(AccountInfo account, CreditCardInfo creditCard)
        {
            SetOverview(account, creditCard);
        }

        private void SetOverview(AccountInfo account, CreditCardInfo creditCard)
        {
            Account = account;
            CreditCard = creditCard;
        }

        public void UpdateAccountInfo(AccountInfo account)
        {
            SetOverview(account, CreditCard);
        }

        public void SetTransactionList(List<TransactionInfo> transactions)
        {
            TransactionList = transactions;
        }

        public void SetTransactionDetailList(List<TransactionDetailInfo> transactionDetails)
        {
            TransactionDetailList = transactionDetails;
        }
    }
}