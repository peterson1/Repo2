using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using PropertyChanged;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Uploader.Lib45.UserControlVMs;

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
}
