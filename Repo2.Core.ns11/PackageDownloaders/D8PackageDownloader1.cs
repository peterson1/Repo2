using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.Compression;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeManagers;

namespace Repo2.Core.ns11.PackageDownloaders
{
    public class D8PackageDownloader1 : IPackageDownloader
    {
        public event EventHandler<StatusText> StatusChanged;

        private IPackagePartManager _parts;
        private IFileArchiver       _archivr;

        public D8PackageDownloader1(IPackagePartManager packagePartManager, IFileArchiver fileArchiver)
        {
            _parts   = packagePartManager;
            _archivr = fileArchiver;
        }


        public async Task<string> DownloadAndUnpack(R2Package remotePackage, string targetDir)
        {
            var list = await _parts.ListByPackage(remotePackage);
            if (list.Count == 0) throw new ArgumentException
                ($"No parts found for package hash “{remotePackage.Hash}”.");

            var orderedList = list.OrderBy(x => x.PartNumber).ToList();

            var paths   = await DownloadPartsToTemp(orderedList);
            return await _archivr.MergeAndDecompress(paths, targetDir);
        }


        private async Task<List<string>> DownloadPartsToTemp(List<R2PackagePart> partsList)
        {
            var paths = new List<string>();
            foreach (var part in partsList)
            {
                StatusChanged.Raise($"Downloading {part.Description} ...");
                paths.Add(await _parts.DownloadToTemp(part));
            }
            return paths;
        }
    }
}
