using Autofac;
using FluentAssertions;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeReaders;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.Uploader.Lib45.Configuration;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.Core.ns11.Tests.NodeReaders
{
    [Trait("Local:453", "Write")]
    public class CachedPkgPartReader1Facts
    {
        private CachedPkgPartReader1 _sut;
        private IR2RestClient        _client;


        public CachedPkgPartReader1Facts()
        {
            using (var scope = Repo2IoC.BeginScope())
            {
                _client = scope.Resolve<IR2RestClient>();
                _sut    = scope.Resolve<CachedPkgPartReader1>();
            }

            var cfg = UploaderConfigFile.Parse(UploaderCfg.Localhost);
            _client.SetCredentials(cfg);
        }


        [Fact]
        public async void ListsPkgByHash1_Uncached()
        {
            var ok = await _client.EnableWriteAccess(null);
            ok.Should().BeTrue("Can write");

        }
    }
}
