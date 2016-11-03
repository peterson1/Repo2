using Autofac;
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
            b.Multi<TestCredentialsChecker>();

            return b.Build();
        }
    }
}
