using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.NodeManagers
{
    public interface IRemotePackageManager
    {
        Task<List<R2Package>> ListByFilename(string pkgFilename, CancellationToken cancelTkn);

        Task<NodeReply>  UpdateRemoteNode  (R2Package updatedPackage, CancellationToken cancelTkn);
        //Task<bool>       IsOutdated        (R2Package localPackage);
    }
}
