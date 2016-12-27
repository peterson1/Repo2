using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using Repo2.AcceptanceTests.Lib;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.SDK.WPF45.Configuration;
using Repo2.UnitTests.Lib.TestTools;
using Repo2.Uploader.Lib45.Configuration;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.SDK.WPF45.Tests.IssuePoster
{
    [Trait("Local:453", "Write")]
    public class D8ErrorTicketManager1Facts
    {
        private IErrorTicketManager _sut;
        private IR2RestClient       _client;
        private R2Credentials       _creds;

        public D8ErrorTicketManager1Facts()
        {
            _creds = UploaderConfigFile.Parse(UploaderCfg.Localhost);
            using (var scope = Repo2IoC.BeginScope())
            {
                _client = scope.Resolve<IR2RestClient>();
                _sut    = scope.Resolve<IErrorTicketManager>();
            }
        }


        [Fact(DisplayName = "Can POST Issues")]
        public async void CanPOSTIssues()
        {
            var ok = await _client.EnableWriteAccess(_creds, new CancellationToken());
            ok.Should().BeTrue("Can write");

            var list = await _sut.List(ErrorState.NeedsReview, new CancellationToken());
            var origCount = list.Count;

            var tkt = GetSampleTicket();
            _sut.Post(tkt);

            await Task.Delay(1000 * 5);

            list = await _sut.List(ErrorState.NeedsReview, new CancellationToken());
            list.Should().HaveCount(origCount + 1);
        }


        private R2ErrorTicket GetSampleTicket()
            => new R2ErrorTicket
            {
                Title = F.ke.Text,
                Description = F.ke.Text,
            };
    }
}
