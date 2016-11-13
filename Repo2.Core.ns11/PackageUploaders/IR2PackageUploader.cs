using System.Threading.Tasks;

namespace Repo2.Core.ns11.PackageUploaders
{
    public interface IR2PackageUploader
    {
        Task<bool>  Upload (string packageFilePath);
    }
}
