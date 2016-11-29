using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.PackageUploaders
{
    public interface IPackageUploader : IStatusChanger
    {
        Task<NodeReply>  Upload  (R2Package localPackage, CancellationToken cancelTkn);

        double MaxPartSizeMB { get; set; }
    }
}
