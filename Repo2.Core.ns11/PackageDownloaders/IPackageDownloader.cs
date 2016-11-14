using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.PackageDownloaders
{
    public interface IPackageDownloader
    {
        Task<string> DownloadAndUnpack(R2Package localPkg, string targetDir);
    }
}
