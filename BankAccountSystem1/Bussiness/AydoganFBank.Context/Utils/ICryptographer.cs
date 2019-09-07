using AydoganFBank.Context.IoC.Lifecycle;

namespace AydoganFBank.Context.Utils
{
    public interface ICryptographer : ISingletonObject
    {
        string GenerateMD5Hash(string input);
    }
}
