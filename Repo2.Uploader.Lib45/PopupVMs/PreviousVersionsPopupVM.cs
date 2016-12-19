﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.NodeManagers;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.InputCommands;

namespace Repo2.Uploader.Lib45.PopupVMs
{
    [ImplementPropertyChanged]
    public class PreviousVersionsPopupVM
    {
        private IPackagePartManager _partsMgr;

        public PreviousVersionsPopupVM(IPackagePartManager packagePartManager)
        {
            _partsMgr = packagePartManager;
        }


        public string   Status            { get; private set; }
        public string   PackageFilename   { get; private set; }

        public ObservableCollection<PackageVersionRowVM> Rows { get; private set; }


        public async void GetOldVersions(string pkgFilename)
        {
            Status          = $"Getting versions for “{pkgFilename}” ...";
            PackageFilename = pkgFilename;

            List<R2PackagePart> parts;
            try
            {
                parts = await _partsMgr.ListByPkgName(pkgFilename, new CancellationToken());
            }
            catch (Exception ex)
            {
                Status = ex.Info();
                return;
            }
            var list      = new List<PackageVersionRowVM>();

            foreach (var verGrp in parts.GroupBy(x => x.PackageHash))
            {
                var row = new PackageVersionRowVM(verGrp, _partsMgr);
                list.Add(row);
            }

            var sortd = list.OrderByDescending(x => x.UploadDate);
            Rows      = new ObservableCollection<PackageVersionRowVM>(sortd);
            Status    = $"Found {list.Count} previous versions.";
        }
    }



    [ImplementPropertyChanged]
    public class PackageVersionRowVM
    {
        private List<R2PackagePart> _parts;
        private IPackagePartManager _partsMgr;

        public PackageVersionRowVM(IGrouping<string, R2PackagePart> grouping, IPackagePartManager packagePartManager)
        {
            _parts           = grouping.ToList();
            _partsMgr        = packagePartManager;
            VersionHash      = grouping.Key;
            UploadDate       = grouping.FirstOrDefault(x => x.PartNumber == 1)?.created;
            DeleteVersionCmd = R2Command.Async(DeleteParts);
            DeleteVersionCmd.DisableWhenDone = true;
        }


        public DateTime?      UploadDate        { get; private set; }
        public string         VersionHash       { get; private set; }
        public IR2Command     DeleteVersionCmd  { get; private set; }
        public string         Status            { get; private set; }

        public CancellationToken CancelToken { get; set; } = new CancellationToken();

        public int Count => _parts?.Count ?? 0;


        private async Task DeleteParts(object arg)
        {
            for (int i = 0; i < Count; i++)
            {
                Status = $"Deleting part {i + 1} of {Count} ...";
                var reply = await _partsMgr.DeleteByPartNid(_parts[i].nid, CancelToken);
                if (reply.Failed)
                {
                    Status = reply.ErrorsText;
                    return;
                }
            }
            Status = $"Successfully deleted all {Count} parts.";
        }


    }
}
