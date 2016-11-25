using System;
using System.IO;
using Autofac;
using FluentAssertions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.PackageDownloaders;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.UnitTests.Lib.TestTools;
using Xunit;

namespace Repo2.UnitTests.Lib.SDK.WPF45.Tests.PackageDownloaders
{
    [Trait("Core", "Unit")]
    public class LocalPackageFileUpdater1Facts
    {
        private ILocalPackageFileUpdater _sut;
        private IFileSystemAccesor       _fileIO;

        public LocalPackageFileUpdater1Facts()
        {
            using (var scope = DownloaderIoC.BeginScope())
            {
                _fileIO = scope.Resolve<IFileSystemAccesor>();
                _sut    = scope.Resolve<ILocalPackageFileUpdater>();
            }
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
            _sut.SetTargetFile(_fileIO.GetTempFilePath());
            var outd8d = await _sut.TargetIsOutdated();
            outd8d.Should().BeFalse();
        }
    }
}
