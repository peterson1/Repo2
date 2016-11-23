using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeManagers;

namespace Repo2.Core.ns11.PackageRegistration
{
    public class D8PreUploadChecker1 : IR2PreUploadChecker
    {
        private IPackageManager _pkgs;

        public D8PreUploadChecker1(IPackageManager packageManager)
        {
            _pkgs   = packageManager;
        }


        public string     ReasonWhyNot  { get; private set; }
        public R2Package  LastPackage   { get; private set; }


        public async Task<bool> IsUploadable(R2Package localPkg)
        {
            if (localPkg == null)
            {
                ReasonWhyNot = $"“{nameof(localPkg)}” argument is NULL.";
                return false;
            }

            if (!localPkg.FileFound)
            {
                ReasonWhyNot = $"“{localPkg.Filename}” not found in {localPkg.LocalDir}.";
                return false;
            }

            var list = await _pkgs.ListByFilename(localPkg);
            if (list == null)
            {
                ReasonWhyNot = "List from server is NULL.";
                return false;
            }
            if (list.Count < 1)
            {
                ReasonWhyNot = "List from server is EMPTY.";
                return false;
            }
            LastPackage = list[0];

            if (LastPackage.Hash == localPkg.Hash)
            {
                ReasonWhyNot = "Local hash matches remote hash.";
                return false;
            }

            return true;
        }
    }
}
