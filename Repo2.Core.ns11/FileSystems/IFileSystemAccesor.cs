using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.FileSystems
{
    public interface IFileSystemAccesor
    {
        string TempDir { get; }

        Task<string> IsolateFile (R2Package localPkg);
        Task         Delete      (params string[] filePaths);
        Task         Delete      (IEnumerable<string> filePaths);
        string       GetSHA1     (string filePath);
    }
}
