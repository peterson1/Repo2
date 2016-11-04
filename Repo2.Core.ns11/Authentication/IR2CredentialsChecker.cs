using System.Threading.Tasks;

namespace Repo2.Core.ns11.Authentication
{
    public interface IR2CredentialsChecker
    {
        Task Check(R2Credentials credentials);
        bool  CanRead   { get; }
        bool  CanWrite  { get; }
    }
}
