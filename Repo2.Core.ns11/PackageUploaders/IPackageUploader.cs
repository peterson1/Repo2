using System;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.PackageUploaders
{
    public interface IPackageUploader
    {
        Task  Upload  (R2Package localPackage);

        double MaxPartSizeMB { get; set; }

        string Status { get; }
    }
}
