using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.NodeManagers
{
    public interface IPackagePartManager
    {
        Task<List<R2PackagePart>>  ListByPackage  (R2Package package, CancellationToken cancelTkn);
        Task<List<R2PackagePart>>  ListByPackage  (string packageFilename, string packageHash, CancellationToken cancelTkn);

        Task<Reply>  AddNode         (R2PackagePart pkgPart, CancellationToken cancelTkn);
        Task<Reply>  DeleteByPackage (R2Package package, CancellationToken cancelTkn);
        Task<string> DownloadToTemp  (R2PackagePart part, CancellationToken cancelTkn);
    }
}
