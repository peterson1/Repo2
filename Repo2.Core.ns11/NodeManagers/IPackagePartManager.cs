using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.NodeManagers
{
    public interface IPackagePartManager
    {
        Task<List<R2PackagePart>>  ListByPkgHash  (R2Package package);
        Task<List<R2PackagePart>>  ListByPkgHash  (R2PackagePart pkgPart);

        Task<Reply>  AddNode         (R2PackagePart pkgPart);
        Task<Reply>  DeleteByPkgHash (R2Package package);
        Task<string> DownloadToTemp  (R2PackagePart part);
    }
}
