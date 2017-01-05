using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeReaders;

namespace Repo2.Core.ns11.NodeManagers
{
    public interface IPackagePartManager : IPackagePartReader
    {
        Task<Reply>      AddNode          (R2PackagePart pkgPart, CancellationToken cancelTkn);
        Task<Reply>      DeleteByPkgHash  (R2Package package, CancellationToken cancelTkn);
        Task<RestReply>  DeleteByPartNid  (int partNodeID, CancellationToken cancelTkn);
        Task<string>     DownloadToTemp   (R2PackagePart part, CancellationToken cancelTkn);
    }
}
