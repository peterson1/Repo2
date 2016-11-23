using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.Compression;
using Repo2.Core.ns11.Exceptions;

namespace Repo2.SDK.WPF45.Compression
{
    public class FileArchiver1 : IFileArchiver
    {
        public Task<IEnumerable<string>> CompressAndSplit
            (string filePath, double maxPartSizeMB)
            => SevenZipper2.CompressAndSplit(filePath, maxPartSizeMB);


        public async Task<string> MergeAndDecompress(IEnumerable<string> orderedPartsPaths, string outputDir)
        {
            var contnt = await SevenZipper2.DecompressFromSplit
                            (orderedPartsPaths, outputDir);

            if (contnt.Count() < 1)
                throw Fault.NoItems("archive contents");

            if (contnt.Count() > 1)
                throw Fault.NonSolo("archive contents", contnt.Count());

            return contnt.Single();
        }
    }
}
