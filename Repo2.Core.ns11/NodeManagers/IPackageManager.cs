using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.NodeManagers
{
    public interface IPackageManager
    {
        Task<Reply> UpdateNode(R2Package localPkg);
    }
}
