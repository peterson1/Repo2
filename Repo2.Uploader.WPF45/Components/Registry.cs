using Autofac;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.RestClients;
using Repo2.SDK.WPF45.TaskResilience;
using Repo2.Uploader.Lib45;

namespace Repo2.Uploader.WPF45.Components
{
    internal class Registry
    {
        internal static IContainer Build()
        {
            var b = new ContainerBuilder();

            b.Solo<MainWindowVM>();

            b.Multi<CrappyConnectionRetryer>();
            b.Multi<IR2RestClient, ResilientClient1>();
            b.Multi<IR2CredentialsChecker, R2D8CredentialsChecker>();
            b.Multi<IR2PackageChecker, R2D8PackageChecker>();

            return b.Build();
        }
    }
}
