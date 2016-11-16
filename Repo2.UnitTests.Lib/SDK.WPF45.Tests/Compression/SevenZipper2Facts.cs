using System;
using System.Linq;
using System.IO;
using FluentAssertions;
using Repo2.Core.ns11.Exceptions;
using Repo2.SDK.WPF45.Compression;
using Repo2.SDK.WPF45.Extensions.FileInfoExtensions;
using Repo2.SDK.WPF45.FileSystems;
using Xunit;

namespace Repo2.UnitTests.Lib.SDK.WPF45.Tests.Compression
{
    [Trait("C:Temp", "write")]
    public class SevenZipper2Facts
    {
        [Fact(DisplayName = "Split: 4 parts")]
        public async void Split4Parts()
        {
            var testFile = "sample_4.6mb";
            var origPath = GetSampleFile(testFile);
            var origHash = origPath.SHA1ForFile();
            var maxSzeMB = 1;

            var paths7z  = await SevenZipper2.CompressAndSplit
                                            (origPath, maxSzeMB);
            paths7z.Should().HaveCount(4);

            var newPaths = await SevenZipper2.DecompressFromSplit
                                            (paths7z, Path.GetTempPath());
            newPaths.Should().HaveCount(1);
            newPaths.First().SHA1ForFile().Should().Be(origHash);

            FileIO.Delete(paths7z);
            FileIO.Delete(newPaths);
        }


        private string GetSampleFile(string filename)
        {
            const string DIR = @"SDK.WPF45.Tests\Compression";
            var topD = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(topD, DIR, filename);
            if (!File.Exists(path))
                throw Fault.Missing(filename, path);
            return path;
        }
    }
}
