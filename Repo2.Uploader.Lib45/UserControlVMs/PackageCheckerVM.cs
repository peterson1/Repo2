using System;
using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.SDK.WPF45.PackageFinders;

namespace Repo2.Uploader.Lib45.UserControlVMs
{
    [ImplementPropertyChanged]
    public class PackageCheckerVM
    {
        public event EventHandler<R2Package> PackageVerified = delegate { };

        private IR2PreUploadChecker _preCheckr;
        private R2Package _pkg;


        public PackageCheckerVM(IR2PreUploadChecker preUploadChecker)
        {
            _preCheckr = preUploadChecker;
        }


        public bool?   IsUploadable  { get; private set; }


        [DependsOn(nameof(IsUploadable))]
        public string ReasonWhyNot => _preCheckr.ReasonWhyNot;


        internal async Task CheckUploadability(string pkgPath)
        {
            IsUploadable = null;
            _pkg = LocalR2Package.From(pkgPath);

            IsUploadable = await _preCheckr.IsUploadable(_pkg, new CancellationToken());

            if (IsUploadable == true)
            {
                _pkg.nid = _preCheckr.LastPackage.nid;
                PackageVerified?.Invoke(this, _pkg);
            }

            //StartUploadCmd.CurrentLabel = IsUploadable
            //    ? "Upload Package" : _preCheckr.ReasonWhyNot;
        }
    }
}
