using Autofac;
using Repo2.AcceptanceTests.Lib.PackageRegistrationSuite;
using Repo2.AcceptanceTests.Lib.UploaderRestrictionsSuite;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.RestClients;
using Repo2.SDK.WPF45.TaskResilience;

namespace Repo2.AcceptanceTests.Lib.ComponentRegistry
{
    internal class ContainerFactory
    {
        internal static IContainer Build(R2Credentials r2Credentials)
        {
            var b = new ContainerBuilder();

            b.RegisterInstance(r2Credentials);

            b.Multi<GetValidUploaderCredentials>();
            b.Multi<CheckUploaderCredentials>();
            b.Multi<CheckPackageRegistration>();

            b.Multi<CrappyConnectionRetryer>();
            b.Multi<IR2RestClient, ResilientClient1>();
            b.Multi<IR2CredentialsChecker, R2D8CredentialsChecker>();
            b.Multi<IR2PreUploadChecker, R2D8PreUploadChecker>();

            return b.Build();
        }
    }
}
