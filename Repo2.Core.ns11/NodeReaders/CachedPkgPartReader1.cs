using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;

namespace Repo2.Core.ns11.NodeReaders
{
    public class CachedPkgPartReader1 : IPackagePartReader
    {
        private const string CACHES_DIR = "Caches";

        private IFileSystemAccesor  _fs;
        private IPackagePartManager _mgr;
        private ILocalDatabase      _db;

        public CachedPkgPartReader1(IPackagePartManager packagePartManager, IFileSystemAccesor fileSystemAccesor, ILocalDatabase localDatabase)
        {
            _fs  = fileSystemAccesor;
            _db  = localDatabase;
            _mgr = packagePartManager;
        }


        public int Hits   { get; private set; }
        public int Misses { get; private set; }


        public async Task<List<R2PackagePart>> ListByPkgHash(R2Package package, CancellationToken cancelTkn)
        {
            await Task.Delay(1);
            return new List<R2PackagePart>();
        }

        public async Task<List<R2PackagePart>> ListByPkgHash(string packageFilename, string packageHash, CancellationToken cancelTkn)
        {
            await Task.Delay(1);
            return new List<R2PackagePart>();
        }

        public async Task<List<R2PackagePart>> ListByPkgName(string packageFilename, CancellationToken cancelTkn)
        {
            if (CacheFound)
            {

            }
            else
            {

            }


            var list = await _mgr.ListByPkgName(packageFilename, cancelTkn);

            _fs.WriteJsonFile(list, CachePath);

            return list;
        }


        public bool CacheFound   => _fs.Found(CachePath);
        public Task ClearCache() => _fs.Delete(CachePath);

        private string CacheFilename => $"{typeof(R2PackagePart).Name}.cache1";
        private string CachePath     => _fs.GetAppDataFilePath(CACHES_DIR, CacheFilename);
    }
}
