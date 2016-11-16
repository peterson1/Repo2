using FluentAssertions;
using Repo2.Core.ns11.DomainModels;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.DomainModels
{
    [Trait("Core", "Unit")]
    public class R2PackagePartValidations
    {
        private R2PackagePart _part;

        public R2PackagePartValidations()
        {
            _part = GetValidPkgPart();
        }


        [Fact(DisplayName = "IsValid() is true if so")]
        public void IsValidIsTrueIfSo()
        {
            _part.IsValid().Should().BeTrue();
        }


        [Fact(DisplayName = "Requires Pkg Filename")]
        public void RequiresPkgFilename()
        {
            _part.PackageFilename = null;
            _part.IsValid().Should().BeFalse();
        }


        [Fact(DisplayName = "Requires Pkg Hash")]
        public void RequiresPkgHash()
        {
            _part.PackageHash = null;
            _part.IsValid().Should().BeFalse();
        }


        [Fact(DisplayName = "Requires Part Hash")]
        public void RequiresPartHash()
        {
            _part.PartHash = null;
            _part.IsValid().Should().BeFalse();
        }


        [Fact(DisplayName = "Valid Total Parts")]
        public void ValidTotalParts()
        {
            _part.TotalParts = 0;
            _part.IsValid().Should().BeFalse();
            _part.TotalParts = -3;
            _part.IsValid().Should().BeFalse();
        }


        [Theory(DisplayName = "Valid Part Number")]
        [InlineData( 3, 2, false)]
        [InlineData( 0, 2, false)]
        [InlineData(-1, 2, false)]
        [InlineData( 1, 2, true)]
        [InlineData( 2, 2, true)]
        public void ValidPartNumber(int partNumbr, int totalParts, bool expctedIsValid)
        {
            _part.TotalParts = totalParts;
            _part.PartNumber = partNumbr;
            _part.IsValid().Should().Be(expctedIsValid);
        }


        private R2PackagePart GetValidPkgPart()
            => new R2PackagePart
            {
                PackageFilename = "sample.pkg",
                PackageHash     = "abc123",
                PartHash        = "cdf456",
                PartNumber      = 1,
                TotalParts      = 2
            };
    }
}
