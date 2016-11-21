﻿using System.Linq;
using FluentAssertions;
using Moq;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;
using Repo2.UnitTests.Lib.TestTools.MoqExtensions;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.PackageRegistration
{
    [Trait("Core", "Unit")]
    public class D8PreUploadCheckerFacts
    {
        [Fact(DisplayName = "Uploadable if registered")]
        public async void Registered()
        {
            var moq = MockClientReturning(R2Pkg("Test.pkg", "v1"));
            var sut = new D8PreUploadChecker1(moq.Object);
            var pkg = R2Pkg("Test.pkg", "v2");
            (await sut.IsUploadable(pkg)).Should().BeTrue();
        }


        [Fact(DisplayName = "Not registered: NO Upload")]
        public async void NotRegistered()
        {
            var moq = MockClientReturning();
            var sut = new D8PreUploadChecker1(moq.Object);
            var pkg = new R2Package("non-registered.pkg");
            (await sut.IsUploadable(pkg)).Should().BeFalse();
        }


        [Fact(DisplayName = "Same hash: NO Upload")]
        public async void SameHash()
        {
            var moq = MockClientReturning(R2Pkg("Pkg.name", "Pkg.hash"));

            var sut = new D8PreUploadChecker1(moq.Object);
            var pkg = R2Pkg("Pkg.name");
            pkg.LocalHash = "Pkg.hash";
            (await sut.IsUploadable(pkg)).Should().BeFalse();
        }


        private Mock<IR2RestClient> MockClientReturning(params R2Package[] r2Packages)
        {
            var moq = new Mock<IR2RestClient>();

            moq.Setup(c => c.List<PackagesByTitle1>(Any.Obj))
                .ReturnsAsync(r2Packages.Select(x => x as PackagesByTitle1).ToList());

            return moq;
        }


        private R2Package R2Pkg(string filename, string remoteHash = null)
            => new R2Package(filename) { RemoteHash = remoteHash };
    }
}
