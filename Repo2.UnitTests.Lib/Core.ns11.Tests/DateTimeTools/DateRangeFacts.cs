using FluentAssertions;
using Repo2.Core.ns11.DateTimeTools;
using System;
using System.Collections.Generic;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.DateTimeTools
{
    [Trait("Core", "Unit")]
    public class DateRangeFacts
    {
        [Fact(DisplayName = "GroupByMonthAndYear: 3 months, same year")]
        public void GroupsByMonthCase1()
        {
            var sut    = new DateRange(7.Feb(2014), 27.Apr(2014));
            var expctd = new List<DateTime>
            {
                1.Feb(2014),
                1.Mar(2014),
                1.Apr(2014),
            };
            sut.GroupByMonthAndYear().Should().Equal(expctd);
        }

        [Fact(DisplayName = "GroupByMonthAndYear: 6 months, same year")]
        public void GroupsByMonthCase2()
        {
            var sut = new DateRange(7.Jun(2014), 27.Nov(2014));
            var expctd = new List<DateTime>
            {
                1.Jun(2014),
                1.Jul(2014),
                1.Aug(2014),
                1.Sep(2014),
                1.Oct(2014),
                1.Nov(2014),
            };
            sut.GroupByMonthAndYear().Should().Equal(expctd);
        }

        [Fact(DisplayName = "GroupByMonthAndYear: up to next year")]
        public void GroupsByMonthCase3()
        {
            var sut = new DateRange(10.Oct(2013), 27.Apr(2014));
            var expctd = new List<DateTime>
            {
                1.Oct(2013),
                1.Nov(2013),
                1.Dec(2013),
                1.Jan(2014),
                1.Feb(2014),
                1.Mar(2014),
                1.Apr(2014),
            };
            sut.GroupByMonthAndYear().Should().Equal(expctd);
        }
    }
}
