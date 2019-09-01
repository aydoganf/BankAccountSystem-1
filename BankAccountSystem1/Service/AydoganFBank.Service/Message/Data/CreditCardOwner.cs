using AydoganFBank.AccountManagement.Api;

namespace AydoganFBank.Service.Message.Data
{
    public class CreditCardOwner
    {
        public int OwnerId { get; set; }
        public CreditCardOwnerType CreditCardOwnerType { get; set; }
        public string AssetsUnit { get; set; }
    }
}