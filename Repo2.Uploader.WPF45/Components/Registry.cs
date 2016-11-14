using Autofac;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
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

            b.Multi<CrappyConnectionRetryer>();
            b.Multi<IR2CredentialsChecker, R2D8CredentialsChecker>();
            b.Multi<IR2PreUploadChecker, R2D8PreUploadChecker>();
            b.Multi<IPackageUploader, R2D8PackageUploader>();

            return b.Build();
        }
    }
}
