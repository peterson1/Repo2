using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.Serialization;

namespace Repo2.Core.ns11.NodeReaders
{
    public class x_CachedPkgPartReader1 : x_CachedReaderBase<R2PackagePart>, IPackagePartReader, x_ICachedReader
    {
        public x_CachedPkgPartReader1(IR2Serializer r2Serializer) : base(r2Serializer)
        {
        }


        public Task<List<R2PackagePart>> ListByPkgHash(R2Package package, CancellationToken cancelTkn)
            => FilterFromCache(package.Filename, package.Hash, cancelTkn);


        public Task<List<R2PackagePart>> ListByPkgHash(string packageFilename, string packageHash, CancellationToken cancelTkn)
            => FilterFromCache(packageFilename, packageHash, cancelTkn);


        public Task<List<R2PackagePart>> ListByPkgName(string packageFilename, CancellationToken cancelTkn)
            => FilterFromCache(packageFilename, null, cancelTkn);


        private async Task<List<R2PackagePart>> FilterFromCache(string pkgFilename, string pkgHash, CancellationToken cancelTkn)
        {
            var cache = await GetMemCache(cancelTkn);

            if (pkgHash.IsBlank())
                return cache.Where(x => x.PackageFilename == pkgFilename).ToList();
            else
                return cache.Where(x => x.PackageFilename == pkgFilename
                                     && x.PackageHash     == pkgHash).ToList();
        }



        protected override Task<IEnumerable<R2PackagePart>> GetFullList(CancellationToken cancelTkn)
        {
            throw new NotImplementedException();
        }
    }
}
