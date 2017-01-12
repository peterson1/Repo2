using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
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
        const string PKG_NAME = "Repo2.TestClient.WPF45.exe";
        const string PKG_HASH = "a07694eb-e213c7d6-3c8e6a8a-a8a06b7d-064349fc";
        private CachedPkgPartReader1 _cache;
        private IPackagePartReader   _readr;
        private IR2RestClient        _client;


        public CachedPkgPartReader1Facts()
        {
            using (var scope = Repo2IoC.BeginScope())
            {
                _client = scope.Resolve<IR2RestClient>();
                _readr  = scope.Resolve<IPackagePartReader>();
                _cache  = _readr as CachedPkgPartReader1;
            }
            var cfg = UploaderConfigFile.Parse(UploaderCfg.Localhost);
            _client.SetCredentials(cfg);
        }


        [Fact(DisplayName = "Creates file on initial call")]
        public async void CreatesFileOnInitialCall()
        {
            await EnableWriteAccess();
            await _cache.ClearCache();
            _cache.CacheFound.Should().BeFalse("must delete all cache files before testing");

            var origHits = _cache.Hits;
            var origMiss = _cache.Misses;
            var list = await _readr.ListByPkgName(PKG_NAME, new CancellationToken());
            list.Count.Should().BeGreaterThan(0, "must return at least 1 record");

            _cache.Hits.Should().Be(origHits, "must retain Hits count");
            _cache.Misses.Should().Be(origMiss + 1, "must increment Misses count");
            _cache.CacheFound.Should().BeTrue("must create cache file");
        }


        private async Task EnableWriteAccess()
        {
            var ok = await _client.EnableWriteAccess(null, false);
            ok.Should().BeTrue("Can write");
        }
    }
}
