using FluentAssertions;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.RestClients;
using Repo2.SDK.WPF45.TaskResilience;
using Repo2.Uploader.Lib45.Configuration;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.PackageUploaderSuite.PreUploadCheckerTests
{
    [Trait("Local:453", "Read")]
    public class PreUploadCheckerFacts
    {
        private IR2RestClient _client;

        public PreUploadCheckerFacts()
        {
            var cfg = LocalConfigFile.Parse(UploaderCfg.KEY);
            var retryr = new CrappyConnectionRetryer();
            _client = new ResilientClient1(retryr);
            _client.SetCredentials(cfg);
            _client.AllowUntrustedCertificate(cfg.CertificateThumb);
        }


        [Fact(DisplayName = "Uploadable if registered")]
        public async void Registered()
        {
            var sut    = new D8PreUploadChecker1(_client);
            var pkg    = new R2Package("Test_Package_1.pkg");
            var actual = await sut.IsUploadable(pkg);
            actual.Should().BeTrue();
        }


        [Fact(DisplayName = "Not registered: NO Upload")]
        public async void NotRegistered()
        {
            var sut    = new D8PreUploadChecker1(_client);
            var pkg    = new R2Package("non-registered.pkg");
            var actual = await sut.IsUploadable(pkg);
            actual.Should().BeFalse();
        }


        [Fact(DisplayName = "Same Hash: NO Upload")]
        public async void SameHash()
        {
            var sut    = new D8PreUploadChecker1(_client);
            var pkg    = new R2Package("non-registered.pkg");
            var actual = await sut.IsUploadable(pkg);
            actual.Should().BeFalse();
        }
    }
}
