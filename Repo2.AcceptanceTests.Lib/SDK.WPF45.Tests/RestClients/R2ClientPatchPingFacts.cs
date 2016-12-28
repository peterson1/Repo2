using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using Repo2.AcceptanceTests.Lib;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.Randomizers;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.SDK.WPF45.Configuration;
using Repo2.Uploader.Lib45.Configuration;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.SDK.WPF45.Tests.RestClients
{
    [Trait("Local:453", "Write")]
    public class R2ClientPatchPingFacts
    {
        private FakeFactory   _fke = new FakeFactory();
        private IR2RestClient _sut;
        private IPingManager  _pings;
        private R2Credentials _creds;

        public R2ClientPatchPingFacts()
        {
            //_creds = UploaderConfigFile.Parse(UploaderCfg.KEY);
            _creds = DownloaderConfigFile.Parse(Downloader1Cfg.KEY);
            using (var scope = Repo2IoC.BeginScope())
            {
                _sut   = scope.Resolve<IR2RestClient>();
                _pings = scope.Resolve<IPingManager>();
            }
        }


        [Fact(DisplayName = "Can PATCH Ping")]
        public async void CanPATCHPing()
        {
            await EnableWriteAccess();
            var pkg     = GetSampleTest1Pkg();
            var newName = _fke.FirstName();
            var ping    = await _pings.GetCurrentUserPing(pkg.Filename, new CancellationToken());
            ping.Should().NotBeNull();
            ping.WindowsAccount.Should().NotBe(newName, "random collission");

            ping.WindowsAccount = newName;
            ping.PingerHash = _fke.Text;

            var reply = await _pings.UpdateNode(ping, new CancellationToken());
            reply.IsSuccessful.Should().BeTrue(reply.ErrorsText);

            var newPing = await _pings.GetCurrentUserPing(pkg.Filename, new CancellationToken());
            newPing.WindowsAccount.Should().Be(newName);
        }


        private R2Package GetSampleTest1Pkg()
            => new R2Package
            {
                Filename = "Test_Package_1.pkg"
            };


        private async Task EnableWriteAccess()
            => (await _sut.EnableWriteAccess(_creds)).Should().BeTrue();
    }
}
