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
        public void TrimEnd(string full, string trim, string expctd)
        {
            full.TrimEnd(trim).Should().Be(expctd);
        }


        [Theory(DisplayName = "HasText")]
        [InlineData("abcdef", "ef", true)]
        [InlineData("abcdef", "abc", true)]
        [InlineData("abcdef", "123", false)]
        [InlineData("abcdef", "abcdefgh", false)]
        public void HasText(string full, string findthis, bool expctd)
        {
            full.HasText(findthis).Should().Be(expctd);
        }
    }
}
