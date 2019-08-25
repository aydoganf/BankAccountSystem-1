namespace aydoganfbank.web.api.bussiness.Inputs.Account
{
    public class TransferAssetsMessage
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public int TransactionType { get; set; }
    }
}
