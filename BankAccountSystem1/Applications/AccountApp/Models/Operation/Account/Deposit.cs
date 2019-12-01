namespace AccountApp.Models.Operation.Account
{
    public class Deposit
    {
        public decimal Amount { get; set; }

        public AccountOverview AccountOverview { get; set; }
    }
}