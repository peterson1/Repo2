using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.Compression;

namespace Repo2.SDK.WPF45.Compression
{
    public class FileArchiver1 : IFileArchiver
    {
        public Task<IEnumerable<string>> CompressAndSplit
            (string filePath, double maxPartSizeMB)
            => SevenZipper2.CompressAndSplit(filePath, maxPartSizeMB);
    }
}
