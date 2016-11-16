using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Moq;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.RestClients;
using Repo2.UnitTests.Lib.TestTools.MoqExtensions;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.NodeManagers
{
    [Trait("Core", "Unit")]
    public class D8PkgPartManager1Facts
    {
        private D8PkgPartManager1 _sut;


        public D8PkgPartManager1Facts()
        {
            _sut = new D8PkgPartManager1(null);
        }


        [Fact(DisplayName = "Rejects blank PkgFilename")]
        public async void RejecstBlank_PkgFilename()
        {
            var part = GetValidPkgPart();
            part.PackageFilename = null;

            await Assert.ThrowsAsync<InvalidDataException>
                (async () => await _sut.AddNode(part));
        }


        [Fact(DisplayName = "Rejects blank PkgHash")]
        public async void RejecstBlank_PkgHash()
        {
            var part = GetValidPkgPart();
            part.PackageHash = null;

            await Assert.ThrowsAsync<InvalidDataException>
                (async () => await _sut.AddNode(part));
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
