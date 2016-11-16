using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.SDK.WPF45.Extensions.FileInfoExtensions;

namespace Repo2.Uploader.Lib45.PackageUploaders
{
    public class PartSender1 : IPartSender
    {
        private IPackagePartManager _partMgr;


        public PartSender1(IPackagePartManager partManager)
        {
            _partMgr = partManager;
        }


        public async Task SendParts(IEnumerable<string> partPaths, R2Package localPkg)
        {
            for (int i = 0; i < partPaths.Count(); i++)
            {
                var path     = partPaths.ElementAt(i);
                var partNode = new R2PackagePart
                {
                    PackageFilename = localPkg.Filename,
                    PackageHash     = localPkg.LocalHash,
                    PartHash        = path.SHA1ForFile(),
                    PartNumber      = i + 1,
                    TotalParts      = partPaths.Count()
                };
                await _partMgr.AddNode(partNode);
            }
        }
    }
}
