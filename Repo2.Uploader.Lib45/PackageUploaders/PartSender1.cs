using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.SDK.WPF45.Extensions.FileInfoExtensions;

namespace Repo2.Uploader.Lib45.PackageUploaders
{
    public class PartSender1 : StatusChanger, IPartSender
    {
        private IPackagePartManager _partMgr;
        private IFileSystemAccesor  _fileIO;

        public PartSender1(IPackagePartManager partManager, IFileSystemAccesor fileSystemAccesor)
        {
            _partMgr = partManager;
            _fileIO  = fileSystemAccesor;
        }


        public async Task SendParts(IEnumerable<string> partPaths, R2Package localPkg, CancellationToken cancelTkn)
        {
            for (int i = 0; i < partPaths.Count(); i++)
            {
                var path     = partPaths.ElementAt(i);
                var partNode = new R2PackagePart
                {
                    PackageFilename = localPkg.Filename,
                    PackageHash     = localPkg.Hash,
                    PartHash        = path.SHA1ForFile(),
                    PartNumber      = i + 1,
                    TotalParts      = partPaths.Count(),
                    Base64Content   = _fileIO.ReadBase64(path)
                };
                SetStatus($"Sending {partNode.Description} ...");
                var reply = await _partMgr.AddNode(partNode, cancelTkn);

                if (reply.Failed)
                    throw new Exception(reply.ErrorsText);
            }
        }
    }
}
