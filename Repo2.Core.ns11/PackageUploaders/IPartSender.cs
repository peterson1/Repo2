using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.PackageUploaders
{
    public interface IPartSender : IStatusChanger
    {
        Task SendParts(IEnumerable<string> partPaths, R2Package localPkg, CancellationToken cancelTkn);
    }
}
