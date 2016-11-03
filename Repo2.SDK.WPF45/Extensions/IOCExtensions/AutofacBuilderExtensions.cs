using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;

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
    }
}
