using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using Repo2.AcceptanceTests.Lib.TestTools;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.SDK.WPF45.Configuration;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.SDK.WPF45.Tests.IssuePoster
{
    [Trait("Local:453", "Write")]
    public class IssuePosterFacts
    {
        IFileSystemAccesor  _fs;
        IErrorTicketManager _errors;

        public IssuePosterFacts()
        {
            var cfg = DownloaderConfigFile.Parse(Downloader1Cfg.KEY);
            using (var scope = Repo2IoC.BeginScope())
            {
                _fs     = scope.Resolve<IFileSystemAccesor>();
                _errors = scope.Resolve<IErrorTicketManager>();
                var cli = scope.Resolve<IR2RestClient>();
                cli.SetCredentials(cfg);
            }
        }


        [Fact(DisplayName = "Posts issue on unhandled error")]
        public async void PostsIssueOnUnhandlederror()
        {
            // create temp file
            var tmpFile = _fs.GetTempFilePath();

            // launch test client that watches the file
            var proc = TestClient.Run(tmpFile);

            var list = await _errors.List(ErrorState.NeedsReview, new CancellationToken());
            var origCount = list.Count;

            // trigger error by deleting watched file
            await Task.Delay(1000 * 5);
            await _fs.Delete(tmpFile);

            // wait a while for issue to be posted
            await Task.Delay(1000 * 5);

            // # of error issues should be plus one
            list = await _errors.List(ErrorState.NeedsReview, new CancellationToken());
            list.Should().HaveCount(origCount + 1);
            var issue = list.Last();

            // error msg should contain the filename
            issue.Description.Should().Contain(tmpFile);

            proc.Kill();
        }
    }
}
