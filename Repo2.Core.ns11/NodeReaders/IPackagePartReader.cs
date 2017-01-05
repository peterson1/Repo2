using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.NodeReaders
{
    public interface IPackagePartReader
    {
        Task<List<R2PackagePart>>  ListByPkgHash  (R2Package package, CancellationToken cancelTkn);
        Task<List<R2PackagePart>>  ListByPkgHash  (string packageFilename, string packageHash, CancellationToken cancelTkn);
        Task<List<R2PackagePart>>  ListByPkgName  (string packageFilename, CancellationToken cancelTkn);
    }
}
