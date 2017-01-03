using Autofac;
using Repo2.Core.ns11.Compression;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageDownloaders;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.Compression;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.FileSystems;
using Repo2.SDK.WPF45.RestClients;
using Repo2.SDK.WPF45.TaskResilience;
using Repo2.Uploader.Lib45.Configuration;
using Repo2.Uploader.Lib45.MainTabVMs;
using Repo2.Uploader.Lib45.PackageUploaders;
using Repo2.Uploader.Lib45.PopupVMs;
using Repo2.Uploader.Lib45.UserControlVMs;

namespace Repo2.Uploader.Lib45.Components
{
    public class UploaderIoC
    {
        public static ILifetimeScope BeginScope()
        {
            var b = new ContainerBuilder();

            b.Solo<MainWindowVM>();
            b.Solo<ConfigLoaderVM>();
            b.Solo<UploaderTabVM>();
            b.Solo<PreviousVerTabVM>();

            b.Solo<AccessCheckerVM>();
            b.Solo<PackageCheckerVM>();
            b.Solo<PackageUploaderVM>();
            b.Solo<PreviousVersionsPopupVM>();

            b.Solo<IR2RestClient, ResilientClient1>();
            b.Solo<IRemotePackageManager, D8RemotePackageMgr1>();
            b.Solo<IPackagePartManager, D8PkgPartManager1>();

            b.Multi<IFileSystemAccesor, FileSystemAccesor1>();
            b.Multi<IFileArchiver, FileArchiver1>();
            b.Multi<IPartSender, PartSender1>();
            b.Multi<CrappyConnectionRetryer>();
            b.Multi<IR2PreUploadChecker, D8PreUploadChecker1>();
            b.Multi<IPackageUploader, D8PackageUploader>();
            b.Multi<IPackageDownloader, D8PackageDownloader1>();

            return b.Build().BeginLifetimeScope();
        }
    }
}
