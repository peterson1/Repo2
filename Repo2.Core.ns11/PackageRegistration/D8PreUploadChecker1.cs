using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeManagers;

namespace Repo2.Core.ns11.PackageRegistration
{
    public class D8PreUploadChecker1 : IR2PreUploadChecker
    {
        private IRemotePackageManager _pkgs;

        public D8PreUploadChecker1(IRemotePackageManager packageManager)
        {
            _pkgs   = packageManager;
        }


        public string     ReasonWhyNot  { get; private set; }
        public R2Package  LastPackage   { get; private set; }


        public async Task<bool> IsUploadable(R2Package localPkg, CancellationToken cancelTkn)
        {
            if (localPkg == null)
            {
                ReasonWhyNot = $"“{nameof(localPkg)}” argument is NULL.";
                return false;
            }

            var nme = localPkg.Filename;
            if (!localPkg.FileFound)
            {
                ReasonWhyNot = $"“{nme}” not found in {localPkg.LocalDir}.";
                return false;
            }

            var list = await _pkgs.ListByFilename(nme, cancelTkn);
            if (list == null)
            {
                ReasonWhyNot = "List from server is NULL.";
                return false;
            }
            if (list.Count < 1)
            {
                ReasonWhyNot = $"Package is not registered: “{nme}”.";
                return false;
            }
            if (list.Count > 1)
            {
                ReasonWhyNot = $"Package is registered {list.Count} times : “{nme}”.";
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
