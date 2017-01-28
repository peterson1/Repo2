using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Repo2.Core.ns11.DateTimeTools;
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
    }
}
