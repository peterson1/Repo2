using System;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.PackageDownloaders
{
    public class D8PackageDownloader1 : IPackageDownloader
    {
        public Task<string> DownloadAndUnpack(R2Package localPkg, string targetDir)
        {
            throw new NotImplementedException();
        }
    }
}
