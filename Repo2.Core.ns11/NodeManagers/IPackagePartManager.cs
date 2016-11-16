using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.NodeManagers
{
    public interface IPackagePartManager
    {
        //R2Package  Package  { get; set; }

        Task AddNode(R2PackagePart pkgPart);
    }
}
