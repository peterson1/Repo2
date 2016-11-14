using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.FileSystems
{
    public interface IFileSplitter
    {
        Task<IEnumerable<string>> Split (string pkgPath, double maxPartSizeMB);
    }
}
