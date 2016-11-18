using Autofac;
using Repo2.Core.ns11.Compression;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.Compression;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.FileSystems;
using Repo2.SDK.WPF45.RestClients;
using Repo2.SDK.WPF45.TaskResilience;
using Repo2.Uploader.Lib45;
using Repo2.Uploader.Lib45.PackageUploaders;

namespace Repo2.Uploader.WPF45.Components
{
    internal class Registry
    {
        internal static IContainer Build()
        {
            var b = new ContainerBuilder();

            b.Solo<MainWindowVM>();
            b.Solo<IR2RestClient, ResilientClient1>();
            b.Solo<IPackagePartManager, D8PkgPartManager1>();

            b.Multi<IFileSystemAccesor, FileSystemAccesor1>();
            b.Multi<IFileArchiver, FileArchiver1>();
            b.Multi<IPartSender, PartSender1>();
            b.Multi<CrappyConnectionRetryer>();
            b.Multi<IR2PreUploadChecker, D8PreUploadChecker1>();
            b.Multi<IPackageUploader, D8PackageUploader>();

            return b.Build();
        }
    }
}
