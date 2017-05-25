using FluentAssertions;
using Repo2.Core.ns11.DateTimeTools;
using System;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.DateTimeExtensions
{
    [Trait("Core", "Unit")]
    public class DateTimeExtensionsFacts
    {
        [Fact(DisplayName = "EachYearUpTo: 7 years")]
        public void EachYearUpToTest1()
        {
            var start = new DateTime(2010, 5, 27);
            var endYr = 2016;
            var list  = start.EachYearUpTo(endYr);

            list.Should().BeEquivalentTo(2016, 2015, 2014, 2013, 2012, 2011, 2010);
            list.Should().BeInDescendingOrder();
        }

        [Fact(DisplayName = "EachYearUpTo: 1 year")]
        public void EachYearUpTo1Yr()
        {
            var start = new DateTime(2010, 5, 27);
            var endYr = 2010;
            var list = start.EachYearUpTo(endYr);

            list.Should().BeEquivalentTo(2010);
        }


        [Fact(DisplayName = "End of Month: 28")]
        public void EndofMonth28() => 7.Feb(2017).EndOfMonth().Should().Be(28.Feb(2017));

        [Fact(DisplayName = "End of Month: 31")]
        public void EndofMonth31() => 8.Mar(2017).EndOfMonth().Should().Be(31.Mar(2017));

        [Fact(DisplayName = "End of Month: 30")]
        public void EndofMonth30() => 9.Apr(2017).EndOfMonth().Should().Be(30.Apr(2017));
    }
}
