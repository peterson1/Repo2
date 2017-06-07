using System;
using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.SDK.WPF45.InputCommands;
using System.ComponentModel;

namespace Repo2.Uploader.Lib45.UserControlVMs
{
    //[ImplementPropertyChanged]
    public class PackageCheckerVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public event EventHandler<R2Package> PackageVerified = delegate { };

        private IR2PreUploadChecker _preCheckr;


        public PackageCheckerVM(IR2PreUploadChecker preUploadChecker)
        {
            _preCheckr = preUploadChecker;

            CheckPackageCmd = R2Command.Async(CheckUploadability,
                          _ => Package != null);
        }


        public string     Text1    { get; private set; }
        public string     Text2    { get; private set; }
        public R2Package  Package  { get; set; }

        public IR2Command CheckPackageCmd { get; private set; }


        private async Task CheckUploadability()
        {
            Text1 = Package.Filename;
            Text2 = "verifying package file ...";

            var ok = await _preCheckr.IsUploadable(Package, new CancellationToken());
            if (!ok)
            {
                Text2 = $"File cannot be uploaded.{L.f}{_preCheckr.ReasonWhyNot}";
                return;
            }

            Package.nid = _preCheckr.LastPackage.nid;
            PackageVerified.Raise(Package);
            Text2 = "Ready for upload";
        }


        internal void Clear()
        {
            Text1   = "";
            Text2   = "";
            Package = null;
        }
    }
}
