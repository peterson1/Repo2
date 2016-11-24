using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.NodeManagers
{
    public interface IPackagePartManager
    {
        Task<List<R2PackagePart>>  ListByPackage  (R2Package package);
        Task<List<R2PackagePart>>  ListByPackage  (string packageFilename, string packageHash);

        Task<Reply>  AddNode         (R2PackagePart pkgPart);
        Task<Reply>  DeleteByPackage (R2Package package);
        Task<string> DownloadToTemp  (R2PackagePart part);
    }
}
