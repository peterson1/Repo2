using System.Threading.Tasks;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.PackageUploaders
{
    public interface IPackageUploader : IStatusChanger
    {
        Task  Upload  (R2Package localPackage);

        double MaxPartSizeMB { get; set; }
    }
}
