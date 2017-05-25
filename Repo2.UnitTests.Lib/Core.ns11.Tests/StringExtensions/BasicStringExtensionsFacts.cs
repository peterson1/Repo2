using FluentAssertions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.StringExtensions
{
    [Trait("Core", "Unit")]
    public class BasicStringExtensionsFacts
    {
        [Theory(DisplayName = "TrimEnd")]
        [InlineData("abcdef", "ef", "abcd")]
        [InlineData("abcdef", "def", "abc")]
        [InlineData("abcdef", "123", "abcdef")]
        public void TestMethod(string full, string trim, string expctd)
        {
            full.TrimEnd(trim).Should().Be(expctd);
        }
    }
}
