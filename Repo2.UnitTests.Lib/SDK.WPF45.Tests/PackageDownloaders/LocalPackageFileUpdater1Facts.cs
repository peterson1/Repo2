using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Moq;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageDownloaders;
using Repo2.Core.ns11.RestClients;
using Repo2.UnitTests.Lib.TestTools;
using Repo2.UnitTests.Lib.TestTools.MoqExtensions;
using Xunit;

namespace Repo2.UnitTests.Lib.SDK.WPF45.Tests.PackageDownloaders
{
    [Trait("Core", "Unit")]
    public class LocalPackageFileUpdater1Facts
    {
        private ILocalPackageFileUpdater    _sut;
        private Mock<IFileSystemAccesor>    _fileIO;
        private Mock<IRemotePackageManager> _pkgs;
        private Mock<IPackageDownloader>    _downlr;
        private Mock<IR2RestClient>         _client;

        public LocalPackageFileUpdater1Facts()
        {
            _fileIO = new Mock<IFileSystemAccesor>();
            _pkgs   = new Mock<IRemotePackageManager>();
            _downlr = new Mock<IPackageDownloader>();
            _client = new Mock<IR2RestClient>();
            _sut    = new LocalPackageFileUpdater1(_pkgs.Object, 
                        _fileIO.Object, _downlr.Object, _client.Object);
        }


        [Fact(DisplayName = "IsOutdated: False if blank target")]
        public async void IsOutdatedErrorifblanktarget()
        {
            var resp = await _sut.TargetIsOutdated(new CancellationToken());
            resp.Should().BeFalse();
        }


        [Fact(DisplayName = "IsOutdated: False if missing file")]
        public async void IsOutdatedErrorifmissingfile()
        {
            _sut.SetTargetFile(F.ke.FilePath);
            var resp = await _sut.TargetIsOutdated(new CancellationToken());
            resp.Should().BeFalse();
        }


        [Fact(DisplayName = "IsOutdated: Error if not in server")]
        public async void IsOutdatedErrorifnotinserver()
        {
            SetPkgsListToReturn();
            SetFileFoundToReturn(true);
            SetFileToR2PackageToReturn(Sample.Package());

            _sut.SetTargetFile(F.ke.FilePath);

            await Assert.ThrowsAsync<MissingMemberException>(async () =>
            {
                await _sut.TargetIsOutdated(new CancellationToken());
            });
        }

        [Fact(DisplayName = "IsOutdated: Error if has duplicates")]
        public async void IsOutdatedErrorifhasduplicates()
        {
            var pkg = Sample.Package();
            SetPkgsListToReturn(pkg, pkg);
            SetFileFoundToReturn(true);
            SetFileToR2PackageToReturn(pkg);

            _sut.SetTargetFile(F.ke.FilePath);

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _sut.TargetIsOutdated(new CancellationToken());
            });
        }


        [Fact(DisplayName = "IsOutdated: False if same hash")]
        public async void IsOutdatedFalseifsamehash()
        {
            var pkg = Sample.Package();
            SetPkgsListToReturn(pkg);
            SetFileFoundToReturn(true);
            SetFileToR2PackageToReturn(pkg);
            _sut.SetTargetFile(F.ke.FilePath);

            var outd8d = await _sut.TargetIsOutdated(new CancellationToken());
            outd8d.Should().BeFalse();
        }


        [Fact(DisplayName = "IsOutdated: True if different hash")]
        public async void IsOutdatedTrueifdifferenthash()
        {
            var remotePkg = Sample.Package();
            var localPkg  = Sample.Package();
            localPkg.Filename = remotePkg.Filename;

            SetFileFoundToReturn(true);
            SetPkgsListToReturn(remotePkg);
            SetFileToR2PackageToReturn(localPkg);

            _sut.SetTargetFile(F.ke.FilePath);

            var outd8d = await _sut.TargetIsOutdated(new CancellationToken());
            outd8d.Should().BeTrue();
        }


        private void SetFileFoundToReturn(bool isFileFound)
            => _fileIO.Setup(x => x.Found(Any.Text)).Returns(isFileFound);

        private void SetFileToR2PackageToReturn(R2Package package)
            => _fileIO.Setup(x => x.ToR2Package(Any.Text)).Returns(package);


        private void SetPkgsListToReturn(params R2Package[] pkgs)
            => _pkgs.Setup(x => x.ListByFilename(Any.Text, Any.Tkn))
                .ReturnsAsync(pkgs.Length == 0 
                    ? new List<R2Package>() : pkgs.ToList());
    }
}
