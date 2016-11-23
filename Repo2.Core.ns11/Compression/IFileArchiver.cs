using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.Compression
{
    public interface IFileArchiver
    {
        Task<IEnumerable<string>>  CompressAndSplit 
            (string filePath, double maxPartSizeMB);

        Task<string> MergeAndDecompress
            (IEnumerable<string> orderedPartsPaths, string outputDir);
    }
}
