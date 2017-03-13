using System;
using System.Collections.Generic;
using FluentAssertions;
using Repo2.Core.ns11.Comparisons;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.Comparisons
{
    [Trait("Core", "Unit")]
    public class PropertyComparerFacts
    {
        [Fact(DisplayName = "compares null Date")]
        public void comparesnullDate()
        {
            var o1   = new TestClass ();//{ Date2 = NullSample };
            var o2   = new TestClass { Date2 = new DateTime?(7.February(2014)) };
            var sut  = new TestComparer(o1, o2);
            var diff = "";
            var same = sut.CompareValues(out diff);
            same.Should().BeFalse();
        }

        //private DateTime? NullSample => 7.February(2014);
        //
        //private DateTime? ToNullable(DateTime date)
        //    => new DateTime?(date);
    }


    class TestClass
    {
        public int        Number1  { get; set; }
        public DateTime   Date1    { get; set; }
        public DateTime?  Date2    { get; set; }
    }


    class TestComparer : PropertyComparerBase<TestClass>
    {
        public TestComparer(TestClass obj1, TestClass obj2) : base(obj1, obj2)
        {
        }

        protected override List<string> GetPropertiesToCompare()
            => new List<string>
            {
                nameof(TestClass.Number1),
                nameof(TestClass.Date1),
                nameof(TestClass.Date2),
            };
    }
}
