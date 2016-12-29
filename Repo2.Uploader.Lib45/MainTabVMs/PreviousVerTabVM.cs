using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Uploader.Lib45.UserControlVMs;

namespace Repo2.Uploader.Lib45.MainTabVMs
{
    [ImplementPropertyChanged]
    public class PreviousVerTabVM
    {
        private IPackagePartManager _partsMgr;

        public PreviousVerTabVM(IPackagePartManager packagePartManager)
        {
            _partsMgr = packagePartManager;
        }


        internal async Task GetVersions(string packageFileName)
        {
            Title    = "finding previous versions ...";
            Filename = packageFileName;

            List<R2PackagePart> parts;
            try
            {
                parts = await _partsMgr.ListByPkgName(packageFileName, new CancellationToken());
            }
            catch (Exception ex)
            {
                Error = ex.Info();
                return;
            }
            var list = new List<PackageVersionRowVM>();

            foreach (var verGrp in parts.GroupBy(x => x.PackageHash))
            {
                var row = new PackageVersionRowVM(verGrp, _partsMgr);
                list.Add(row);
            }

            var sortd = list.OrderByDescending(x => x.UploadDate);
            Rows      = new Observables<PackageVersionRowVM>(sortd);
            Title     = $"Previous Versions ({list.Count})";
        }


        public string   Title      { get; private set; }
        public string   Error      { get; private set; }
        public string   Filename   { get; private set; }

        public Observables<PackageVersionRowVM>  Rows  { get; private set; }
    }
}
