using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.UnitTests.Lib.TestTools;
using Repo2.Uploader.Lib45.Configuration;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.D8ServerTests.R2TweaksTests
{
    [Trait("Local:453", "Write")]
    public class ForceRevisionFacts
    {
        const int    NODE_ID    = 1;
        const string NODE_TITLE = "Test_Package_1.pkg";
        private IR2RestClient         _client;
        private IRemotePackageManager _pkgMgr;

        public ForceRevisionFacts()
        {
            using (var scope = Repo2IoC.BeginScope())
            {
                _client = scope.Resolve<IR2RestClient>();
                _pkgMgr = scope.Resolve<IRemotePackageManager>();
            }

            var cfg = UploaderConfigFile.Parse(UploaderCfg.Localhost);
            _client.SetCredentials(cfg);
        }


        [Fact(DisplayName = "Package edit creates revision")]
        public async void PackageEditCreatesRevision()
        {
            await EnableWriteAccess();

            var origCount = await CountCurrentRevisions();

            await UpdatePackageNode();

            var newCount = await CountCurrentRevisions();

            newCount.Should().Be(origCount + 1);
        }

        private async Task EnableWriteAccess()
        {
            var ok = await _client.EnableWriteAccess(null, false);
            ok.Should().BeTrue("Can write");
        }


        private async Task<int> CountCurrentRevisions()
        {
            var revs = await _client.ListRevisions(NODE_TITLE, new CancellationToken());
            return revs.Count;
        }


        private async Task UpdatePackageNode()
        {
            var node = new R2Package
            {
                nid = NODE_ID,
                Filename = NODE_TITLE,
                Hash = F.ke.Text
            };
            //var list = await _pkgMgr.ListByFilename(NODE_TITLE, new CancellationToken());
            //list.Should().HaveCount(1);
            //var node  = list[0];
            //node.Hash = F.ke.Text;

            var reply = await _pkgMgr.UpdateRemoteNode(node, F.ke.FullName(), new CancellationToken());
            reply.IsSuccessful.Should().BeTrue("reply.IsSuccessful");
        }
    }
}
