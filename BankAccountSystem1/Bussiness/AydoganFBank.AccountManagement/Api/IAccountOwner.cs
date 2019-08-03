namespace AydoganFBank.AccountManagement.Api
{
    public interface IAccountOwner
    {
        AccountOwnerType OwnerType { get; }
        int OwnerId { get; }
    }
}
