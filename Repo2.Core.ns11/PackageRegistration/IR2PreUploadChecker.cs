using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.PackageRegistration
{
    public interface IR2PreUploadChecker
    {
        Task<bool>  IsUploadable  (R2Package localPackage);

        string ReasonWhyNot { get; }
    }
}
