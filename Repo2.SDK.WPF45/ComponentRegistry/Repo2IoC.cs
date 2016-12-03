using System.Windows;
using Autofac;
using Repo2.Core.ns11.Compression;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageDownloaders;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.Compression;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.FileSystems;
using Repo2.SDK.WPF45.RestClients;
using Repo2.SDK.WPF45.TaskResilience;

namespace Repo2.SDK.WPF45.ComponentRegistry
{
    public class Repo2IoC
    {
        public static ILifetimeScope BeginScope()
        {
            var b = new ContainerBuilder();

            RegisterComponentsTo(ref b);

            return b.Build().BeginLifetimeScope();
        }


        public static ILifetimeScope BeginScope(Application appToBeErrorHandled = null, string configKey = null)
        {
            if (appToBeErrorHandled != null && !configKey.IsBlank())
                UnhandledErrors.CatchFor(appToBeErrorHandled, configKey);

            return BeginScope();
        }


        public static void RegisterComponentsTo(ref ContainerBuilder b)
        {
            b.Solo<IR2RestClient, ResilientClient1>();
            b.Solo<IRemotePackageManager, D8RemotePackageMgr1>();
            b.Solo<IPackagePartManager, D8PkgPartManager1>();
            b.Solo<IPingManager, D8PingManager1>();
            b.Solo<IErrorTicketManager, D8ErrorTicketManager1>();

            b.Multi<ILocalPackageFileUpdater, LocalPackageFileUpdater1>();
            b.Multi<IFileSystemAccesor, FileSystemAccesor1>();
            b.Multi<IFileArchiver, FileArchiver1>();
            b.Multi<CrappyConnectionRetryer>();
            b.Multi<IPackageDownloader, D8PackageDownloader1>();
        }
    }
}
