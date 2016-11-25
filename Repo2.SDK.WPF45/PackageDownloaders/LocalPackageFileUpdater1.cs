using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageDownloaders;
using Repo2.SDK.WPF45.PackageFinders;

namespace Repo2.SDK.WPF45.PackageDownloaders
{
    public class LocalPackageFileUpdater1 : LocalPackageFileUpdaterBase
    {
        public LocalPackageFileUpdater1(IRemotePackageManager remotePackageManager, IFileSystemAccesor fileSystemAccesor) : base(remotePackageManager, fileSystemAccesor)
        {
        }

        public override R2Package R2PackageFromFile(string targetPath)
            => LocalR2Package.From(targetPath);
    }
}
