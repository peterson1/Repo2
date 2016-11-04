using Autofac;
using Repo2.AcceptanceTests.Lib.UploaderRestrictionsSuite;
using Repo2.Core.ns11.Authentication;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;

namespace Repo2.AcceptanceTests.Lib.ComponentRegistry
{
    internal class ContainerFactory
    {
        internal static IContainer Build()
        {
            var b = new ContainerBuilder();

            b.Multi<GetValidUploaderCredentials>();
            b.Multi<CheckUploaderCredentials>();
            b.Multi<IR2CredentialsChecker, TestCredentialsChecker>();

            return b.Build();
        }
    }
}
