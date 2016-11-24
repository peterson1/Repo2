using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;
using Repo2.UnitTests.Lib.TestTools;
using Repo2.UnitTests.Lib.TestTools.MoqExtensions;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.NodeManagers
{
    [Trait("Core", "Unit")]
    public class D8PackageManager1Facts
    {
        [Fact(DisplayName = "IsOutdated: Yes if diff hash")]
        public async void HasUpdatesTrue_IfDifferentHash()
        {
            var pkg1  = Sample.Package();
            var moq   = MoqClientReturning(pkg1);
            var sut   = new D8PackageManager1(moq.Object);

            var pkg2  = new R2Package(pkg1.Filename);
            pkg2.Hash = "different from " + pkg1.Hash;

            var upd8d = await sut.IsOutdated(pkg2);
            upd8d.Should().BeTrue();
        }


        [Fact(DisplayName = "IsOutdated: No if same hash")]
        public async void HasUpdatesNo_IfSameHash()
        {
            var pkg1  = Sample.Package();
            var moq   = MoqClientReturning(pkg1);
            var sut   = new D8PackageManager1(moq.Object);

            var pkg2  = new R2Package(pkg1.Filename);
            pkg2.Hash = pkg1.Hash; // same hash

            var upd8d = await sut.IsOutdated(pkg2);
            upd8d.Should().BeFalse();
        }


        [Fact(DisplayName = "IsOutdated: No if not in server")]
        public async void HasUpdatesNo_IfNotInServer()
        {
            var moq   = MoqClientReturning();
            var sut   = new D8PackageManager1(moq.Object);

            var upd8d = await sut.IsOutdated(Sample.Package());
            upd8d.Should().BeFalse();
        }


        [Fact(DisplayName = "IsOutdated: Error if has duplicates")]
        public async void HasUpdatesError_IfHasDuplicates()
        {
            var pkg = Sample.Package();
            var moq = MoqClientReturning(pkg, pkg);
            var sut = new D8PackageManager1(moq.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await sut.IsOutdated(pkg);
            });
        }


        private Mock<IR2RestClient> MoqClientReturning(params PackagesByFilename1[] packages)
        {
            var moq = new Mock<IR2RestClient>();

            moq.Setup(c => c.List<PackagesByFilename1>(Any.Text))
                .ReturnsAsync(packages.ToList());

            return moq;
        }
    }
}
