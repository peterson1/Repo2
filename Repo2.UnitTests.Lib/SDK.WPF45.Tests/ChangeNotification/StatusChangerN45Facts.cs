using FluentAssertions;
using Repo2.SDK.WPF45.ChangeNotification;
using Xunit;

namespace Repo2.UnitTests.Lib.SDK.WPF45.Tests.ChangeNotification
{
    [Trait("Core", "Unit")]
    public class StatusChangerN45Facts
    {
        [Fact(DisplayName = "StatusChanged won't fire twice")]
        public void StatusChangedWontFireTwice()
        {
            var changr = new TestChanger();
            var countr = new TestCounter();

            var testC1 = new TestClass(changr, countr);
            var testC2 = new TestClass(changr, countr);

            changr.RaiseStatusChange();

            countr.CallCount.Should().Be(1);
        }


        class TestChanger : StatusChangerN45
        {
            public void RaiseStatusChange()
                => SetStatus("abc");
        }


        class TestClass
        {
            public TestClass(StatusChangerN45 changr, TestCounter countr)
            {
                changr.StatusChanged += (s, e) => countr.CallCount++;
            }
        }


        class TestCounter
        {
            public int CallCount { get; set; }
        }
    }
}
