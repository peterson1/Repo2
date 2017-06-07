using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.NodeManagers;
using Repo2.SDK.WPF45.InputCommands;
using Repo2.Uploader.Lib45.UserControlVMs;
using System.ComponentModel;

namespace Repo2.Uploader.Lib45.MainTabVMs
{
    //[ImplementPropertyChanged]
    public class PreviousVerTabVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private IPackagePartManager _partsMgr;

        public PreviousVerTabVM(IPackagePartManager packagePartManager)
        {
            _partsMgr = packagePartManager;

            GetVersionsCmd = R2Command.Async(GetVersions, 
                         _ => !Filename.IsBlank());
        }


        public string      Filename       { get; private set; }
        public string      Title          { get; private set; } = "...";
        public string      Error          { get; private set; }
        public IR2Command  GetVersionsCmd { get; private set; }

        public Observables<PackageVersionRowVM>  Rows  { get; private set; }


        private async Task GetVersions()
        {
            Title    = "finding previous versions ...";

            List<R2PackagePart> parts;
            try
            {
                parts = await _partsMgr.ListByPkgName(Filename, new CancellationToken());
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


        internal void SetPackage(string pkgFilename)
        {
            Clear();
            Filename = pkgFilename;
            GetVersionsCmd.ExecuteIfItCan();
        }


        private void Clear()
        {
            Filename = "";
            Title    = "...";
            Error    = "";
            Rows?.Clear();
        }
    }
}
