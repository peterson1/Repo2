using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.NodeManagers
{
    public interface IPackageManager
    {
        Task<List<R2Package>> ListByFilename(R2Package package);

        Task<NodeReply> UpdateNode (R2Package updatedPackage);
    }
}
