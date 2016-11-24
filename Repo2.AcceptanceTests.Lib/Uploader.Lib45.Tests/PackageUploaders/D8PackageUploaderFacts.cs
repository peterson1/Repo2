﻿using System.IO;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FluentAssertions;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.Core.ns11.Randomizers;
using Repo2.Core.ns11.RestClients;
using Repo2.Uploader.Lib45.Components;
using Repo2.Uploader.Lib45.Configuration;
using Repo2.Uploader.Lib45.PackageFinders;
using Xunit;

namespace Repo2.AcceptanceTests.Lib.Uploader.Lib45.Tests.PackageUploaders
{
    [Trait("Local:453", "Write")]
    public class D8PackageUploaderFacts
    {
        private FakeFactory         _fke = new FakeFactory();
        private R2Credentials       _creds;
        private IPackagePartManager _parts;
        private IPackageManager     _pkgs;
        private IR2RestClient       _client;
        private IPackageUploader    _sut;

        public D8PackageUploaderFacts()
        {
            _creds = UploaderConfigFile.Parse(UploaderCfg.KEY);
            using (var scope = UploaderIoC.BeginScope())
            {
                _client = scope.Resolve<IR2RestClient>();
                _sut    = scope.Resolve<IPackageUploader>();
                _parts  = scope.Resolve<IPackagePartManager>();
                _pkgs   = scope.Resolve<IPackageManager>();
            }
        }

        [Theory(DisplayName = "Upload multi-parts")]
        [InlineData(6, 0.5, 7)]
        [InlineData(2, 0.5, 3)]
        public async void TestPackage2_6MB(double srcMB, double maxMB, int parts)
        {
            var pkg = CreateFileWithSizeMB(srcMB);
            await EnableWriteAccess();
            await _parts.DeleteByPkgHash(pkg);

            _sut.MaxPartSizeMB = maxMB;
            await _sut.Upload(pkg);

            var list1 = await _parts.ListByPkgHash(pkg);
            list1.Should().HaveCount(parts);

            var pkgs = await _pkgs.ListByFilename(pkg);
            pkgs.Should().HaveCount(1);
            pkgs[0].Hash.Should().Be(pkg.Hash);
        }

        private async Task EnableWriteAccess()
        {
            var ok = await _client.EnableWriteAccess(_creds);
            ok.Should().BeTrue();
        }

        private R2Package CreateFileWithSizeMB(double fileSizeMB)
        {
            var nme  = $"sampleFile.{fileSizeMB}MB";
            var tmp1 = Path.Combine(Path.GetTempPath(), nme);
            var tmp2 = Path.Combine(Path.GetTempPath(), "Test_Package_2.pkg");
            var size = (int)(fileSizeMB * 1024 * 1024);

            if (!File.Exists(tmp1))
            {
                var sb = new StringBuilder();

                while (sb.Length < size)
                    sb.AppendLine(_fke.Text);

                File.WriteAllText(tmp1, sb.ToString());
            }

            File.Copy(tmp1, tmp2, true);

            var pkg = LocalR2Package.From(tmp2);
            pkg.nid = 2;
            return pkg;
        }
    }
}
