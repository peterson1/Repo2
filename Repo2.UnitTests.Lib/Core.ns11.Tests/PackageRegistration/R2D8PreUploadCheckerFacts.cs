using System.Linq;
using FluentAssertions;
using Moq;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.RestClients;
using Repo2.UnitTests.Lib.TestTools.MoqExtensions;
using Xunit;

namespace Repo2.UnitTests.Lib.Core.ns11.Tests.PackageRegistration
{
    [Trait("Core", "Unit")]
    public class R2D8PreUploadCheckerFacts
    {
        private Mock<IR2RestClient> _moq;

        public R2D8PreUploadCheckerFacts()
        {
            _moq = new Mock<IR2RestClient>();
        }


        [Fact(DisplayName = "Uploadable if registered")]
        public async void Registered()
        {
            ServerWillReturn(R2Pkg("Test.pkg", "v1"));

            var sut = new D8PreUploadChecker1(_moq.Object);
            var pkg = R2Pkg("Test.pkg", "v2");
            (await sut.IsUploadable(pkg)).Should().BeTrue();
        }


        [Fact(DisplayName = "Not registered: NO Upload")]
        public async void NotRegistered()
        {
            ServerWillReturn();

            var sut = new D8PreUploadChecker1(_moq.Object);
            var pkg = new R2Package("non-registered.pkg");
            (await sut.IsUploadable(pkg)).Should().BeFalse();
        }


        [Fact(DisplayName = "Same hash: NO Upload")]
        public async void SameHash()
        {
            ServerWillReturn(R2Pkg("Pkg.name", "Pkg.hash"));

            var sut = new D8PreUploadChecker1(_moq.Object);
            var pkg = R2Pkg("Pkg.name");
            pkg.LocalHash = "Pkg.hash";
            (await sut.IsUploadable(pkg)).Should().BeFalse();
        }


        private void ServerWillReturn(params R2Package[] r2Packages)
        {
            _moq.Setup(c => c.BasicAuthList<R2Package>(Any.Text, Any.Text))
                .ReturnsAsync(r2Packages.ToList());
        }


        private R2Package R2Pkg(string filename, string remoteHash = null)
            => new R2Package(filename) { RemoteHash = remoteHash };
    }
}
