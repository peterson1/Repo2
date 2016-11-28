using System;
using System.IO;
using Autofac;
using FluentAssertions;
using Repo2.AcceptanceTests.Lib.TestTools;
using Repo2.Core.ns11.PackageDownloaders;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.SDK.WPF45.PackageFinders;
using Repo2.Uploader.Lib45.Components;
using Repo2.Uploader.Lib45.Configuration;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.IntegrationTests
{
    [Trait("Local:453", "Write")]
    public class PackageUpdaterFacts
    {
        private ILocalPackageFileUpdater _downldr;

        public PackageUpdaterFacts()
        {
            IR2RestClient client;
            var cfg = UploaderConfigFile.Parse(UploaderCfg.KEY);
            using (var scope = DownloaderIoC.BeginScope())
            {
                client   = scope.Resolve<IR2RestClient>();
                _downldr = scope.Resolve<ILocalPackageFileUpdater>();
            }
            client.SetCredentials(cfg);
        }


        [Fact(DisplayName = "Can Update Running Exe")]
        public async void UpdateRunningExe()
        {
            var proc     = TestClient.Run();
            var exePath  = proc.StartInfo.FileName;
            var localPkg = LocalR2Package.From(exePath);
            var evtRaisd = false;
            localPkg.nid = 29;

            Assert.Throws<UnauthorizedAccessException>(() 
                => File.Delete(exePath));

            _downldr.SetTargetFile(exePath);
            var isOld = await _downldr.TargetIsOutdated();
            isOld.Should().BeTrue("test should start with outdated target");

            _downldr.TargetUpdated += (s, e) => evtRaisd = true;
            await _downldr.UpdateTarget();

            isOld = await _downldr.TargetIsOutdated();
            isOld.Should().BeFalse("target should now be up-to-date");

            evtRaisd.Should().BeTrue();

            proc.Kill();
        }



    }
}
