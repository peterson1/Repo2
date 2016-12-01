using System.Threading.Tasks;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.PackageUploaders
{
    public interface IPackageUploader : IStatusChanger
    {
        Task<NodeReply>  StartUpload  (R2Package localPackage);
        void             StopUpload   ();

        bool   IsUploading   { get; }
        double MaxPartSizeMB { get; set; }
    }
}
