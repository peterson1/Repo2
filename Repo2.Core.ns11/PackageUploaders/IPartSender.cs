using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.PackageUploaders
{
    public interface IPartSender
    {
        Task SendParts(IEnumerable<string> partPaths, R2Package localPkg);
    }
}
