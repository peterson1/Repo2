using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.PackageDownloaders
{
    public interface IPackageDownloader : IStatusChanger
    {
        Task<string> DownloadAndUnpack(R2Package remotePackage, string targetDir, CancellationToken cancelTkn);
    }
}
