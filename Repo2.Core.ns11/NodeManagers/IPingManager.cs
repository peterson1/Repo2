using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.NodeManagers
{
    public interface IPingManager
    {
        Task<R2Ping>     GetCurrentUserPing  (string pkgFilename, CancellationToken cancelTkn);
        Task<NodeReply>  UpdateNode          (R2Ping ping, CancellationToken cancelTkn);
    }
}
