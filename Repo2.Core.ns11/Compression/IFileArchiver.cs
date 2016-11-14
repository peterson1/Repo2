using System.Threading.Tasks;

namespace Repo2.Core.ns11.Compression
{
    public interface IFileArchiver
    {
        Task  CompressInPlace (string filePath);
    }
}
