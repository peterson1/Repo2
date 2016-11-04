using System;
using FluentAssertions;
using Repo2.Core.ns11.ReflectionTools;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.ReflectionTools
{
    public class AssemblyReflectorFacts
    {
        [Fact(DisplayName = "Prepends namespace to type name in same assembly")]
        public void SameAssembly()
        {
            var typ = this.GetType();
            var actual = typ.Assembly.PrependNamespace(typ.Name);
            actual.Should().Be($"{typ.Namespace}.{typ.Name}");
        }


        [Fact(DisplayName = "Prepends namespace to type name in another assembly")]
        public void DiffAssembly()
        {
            var typ = typeof(AssemblyReflector);
            var actual = typ.Assembly.PrependNamespace(typ.Name);
            actual.Should().Be($"{typ.Namespace}.{typ.Name}");
        }


        [Fact(DisplayName = "Can suppress error if missing")]
        public void NoErrorIfMissing()
        {
            var typ = GetType().Assembly.FindTypeByName("sdff", false);
            typ.Should().BeNull();
        }


        [Fact(DisplayName = "Throws error if missing")]
        public void ErrorIfMissing()
        {
            Type typ = null;

            Assert.Throws<TypeAccessException>(() =>
            {
                typ = GetType().Assembly.FindTypeByName("sdff", true);
            });

            typ.Should().BeNull();
        }
    }
}
