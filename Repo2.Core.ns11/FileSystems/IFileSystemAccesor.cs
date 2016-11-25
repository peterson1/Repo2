using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.FileSystems
{
    public interface IFileSystemAccesor
    {
        string TempDir { get; }

        Task<string> IsolateFile   (R2Package localPkg);
        Task<bool>   Delete        (params string[] filePaths);
        Task<bool>   Delete        (IEnumerable<string> filePaths);
        string       GetSHA1       (string filePath);
        string       WriteTempFile (byte[] byts);

        string       GetTempFilePath (string filename = null);

        string       ReadBase64    (string filePath);
        bool         Found         (string filePath);
    }
}
