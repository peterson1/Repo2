using FluentAssertions;
using Repo2.Core.ns11.ReflectionTools;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.ReflectionTools
{
    [Trait("Core", "Unit")]
    public class NestedPropertyFinderFacts
    {
        [Fact(DisplayName = "Non-nested property")]
        public void Case1()
        {
            var typ = typeof(ISample1);
            var prop = typ.FindProperty(nameof(ISample1.NonNested));
            prop.Should().NotBeNull();
            prop.Name.Should().Be(nameof(ISample1.NonNested));
        }


        [Fact(DisplayName = "1-level Nested property")]
        public void Case2()
        {
            var typ = typeof(ISampleChild);
            var prop = typ.FindProperty(nameof(ISampleParent.ParentProp));
            prop.Should().NotBeNull();
            prop.Name.Should().Be(nameof(ISampleParent.ParentProp));
        }


        interface ISample1
        {
            int NonNested { get; }
        }

        interface ISampleChild : ISampleParent
        {
            int ChildProp { get; }
        }

        interface ISampleParent
        {
            int ParentProp { get; }
        }
    }
}
