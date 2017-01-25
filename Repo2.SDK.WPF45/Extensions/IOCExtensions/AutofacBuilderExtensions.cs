using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.SDK.WPF45.Extensions.IOCExtensions
{
    public static class AutofacBuilderExtensions
    {
        public static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> Solo<T>(this ContainerBuilder buildr)
            => buildr.RegisterType<T>().AsSelf().SingleInstance();

        public static IRegistrationBuilder<TConcrete, ConcreteReflectionActivatorData, SingleRegistrationStyle> Solo<TInterface, TConcrete>(this ContainerBuilder buildr) where TConcrete : TInterface
            => buildr.RegisterType<TConcrete>().As<TInterface>().SingleInstance();

        public static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> Multi<T>(this ContainerBuilder buildr)
            => buildr.RegisterType<T>().AsSelf();

        public static IRegistrationBuilder<TConcrete, ConcreteReflectionActivatorData, SingleRegistrationStyle> Multi<TInterface, TConcrete>(this ContainerBuilder buildr) where TConcrete : TInterface
            => buildr.RegisterType<TConcrete>().As<TInterface>();


        public static string GetMessage(this DependencyResolutionException ex)
        {
            if (ex.InnerException == null)
                return ex.Message;

            if (ex.InnerException.InnerException == null)
                return ex.InnerException.Message;

            var msg = ex.InnerException.InnerException.Message;
            var resolving = msg.Between("DefaultConstructorFinder' on type '", "'");
            var argTyp = msg.Between("Cannot resolve parameter '", " ");
            var argNme = msg.Between(argTyp + " ", "'");
            return $"Check constructor of :{L.f}‹{resolving}›{L.F}"
                 + $"Can't resolve argument “{argNme}” of type :{L.f}‹{argTyp}›";
        }
    }
}
