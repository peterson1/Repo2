using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;

namespace Repo2.Core.ns11.PackageRegistration
{
    public interface IR2PackageChecker
    {
        Task<bool> IsRegistered(string packageName, R2Credentials credentials);
    }
}
