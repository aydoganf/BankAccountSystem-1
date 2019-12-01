namespace AccountApp.Models.Operation.Account
{
    public class Withdraw
    {
        public decimal Amount { get; set; }

        public AccountOverview AccountOverview { get; set; }
    }
}