using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Moq;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageDownloaders;
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

        public LocalPackageFileUpdater1Facts()
        {
            _fileIO = new Mock<IFileSystemAccesor>();
            _pkgs   = new Mock<IRemotePackageManager>();
            _sut    = new LocalPackageFileUpdater1(_pkgs.Object, _fileIO.Object, null);
        }


        [Fact(DisplayName = "IsOutdated: Error if blank target")]
        public async void IsOutdatedErrorifblanktarget()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _sut.TargetIsOutdated();
            });
        }


        [Fact(DisplayName = "IsOutdated: Error if missing file")]
        public async void IsOutdatedErrorifmissingfile()
        {
            _sut.SetTargetFile(F.ke.FilePath);

            await Assert.ThrowsAsync<FileNotFoundException>(async () =>
            {
                await _sut.TargetIsOutdated();
            });
        }


        [Fact(DisplayName = "IsOutdated: False if not in server")]
        public async void IsOutdatedFalseifnotinserver()
        {
            SetPkgsListToReturn();
            SetFileFoundToReturn(true);
            SetFileToR2PackageToReturn(Sample.Package());

            _sut.SetTargetFile(F.ke.FilePath);
            var outd8d = await _sut.TargetIsOutdated();
            outd8d.Should().BeFalse();
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
                await _sut.TargetIsOutdated();
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

            var outd8d = await _sut.TargetIsOutdated();
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

            var outd8d = await _sut.TargetIsOutdated();
            outd8d.Should().BeTrue();
        }


        private void SetFileFoundToReturn(bool isFileFound)
            => _fileIO.Setup(x => x.Found(Any.Text)).Returns(isFileFound);

        private void SetFileToR2PackageToReturn(R2Package package)
            => _fileIO.Setup(x => x.ToR2Package(Any.Text)).Returns(package);


        private void SetPkgsListToReturn(params R2Package[] pkgs)
            => _pkgs.Setup(x => x.ListByFilename(Any.Text))
                .ReturnsAsync(pkgs.Length == 0 
                    ? new List<R2Package>() : pkgs.ToList());
    }
}
