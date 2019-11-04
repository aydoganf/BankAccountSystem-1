using AydoganFBank.Service.Dispatcher.Data;

namespace AccountApp.Models
{
    public class AccountOverview
    {
        public AccountInfo Account { get; set; }
        private CreditCardInfo CreditCard { get; set; }

        public string AssetsUnit { get; set; }
        public decimal AccountBalance { get; set; }
        public decimal Dept { get; set; }
        public decimal TotalAsset { get; set; }
        public decimal DeptBalanceRatio { get; set; }
        public string AccountBalanceWithAssetsUnit { get; set; }
        public string DeptWithAssetsUnit { get; set; }
        public string TotalAssetWithAssetsUnit { get; set; }

        public AccountOverview(AccountInfo account, CreditCardInfo creditCard)
        {
            Account = account;
            CreditCard = creditCard;

            AssetsUnit = account.AccountType.AssetsUnit;
            AccountBalance = Account.Balance;
            Dept = CreditCard != null ? CreditCard.Debt : 0;
            TotalAsset = AccountBalance - Dept;

            AccountBalanceWithAssetsUnit = $"{AccountBalance} {Account.AccountType.AssetsUnit}";
            DeptWithAssetsUnit = $"{Dept} {Account.AccountType.AssetsUnit}";
            TotalAssetWithAssetsUnit = $"{TotalAsset} {Account.AccountType.AssetsUnit}";

            if (TotalAsset <= 0)
            {
                DeptBalanceRatio = 100;
            }
            else
            {
                DeptBalanceRatio = Dept / AccountBalance;
            }
        }
    }
}