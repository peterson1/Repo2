using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.PackageRegistration
{
    public interface IR2PreUploadChecker
    {
        Task<bool>  IsUploadable  (R2Package localPackage, CancellationToken cancelTkn);

        R2Package LastPackage { get; }

        string ReasonWhyNot { get; }
    }
}
