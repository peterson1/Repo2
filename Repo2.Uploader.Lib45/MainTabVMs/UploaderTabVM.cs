using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.Uploader.Lib45.Configuration;
using Repo2.Uploader.Lib45.PopupVMs;
using Repo2.Uploader.Lib45.UserControlVMs;

namespace Repo2.Uploader.Lib45.MainTabVMs
{
    [ImplementPropertyChanged]
    public class UploaderTabVM
    {
        private string _pkgPath;

        public UploaderTabVM(IR2RestClient restClient,
                             IPackageUploader packageUploader,
                             PreviousVersionsPopupVM previousVersionsPopupVM,
                             AccessCheckerVM accessCheckerVM,
                             PackageCheckerVM packageCheckerVM,
                             PackageUploaderVM packageUploaderVM)
        {
            PkgChecker    = packageCheckerVM;
            AccessChecker = accessCheckerVM;
            PkgUploader   = packageUploaderVM;
            Previous      = previousVersionsPopupVM;

            restClient.OnRetry += (s, e)
                => ClientStatus += $"{L.f}{e}";

            //ConfigLoader.PackagePathChanged += async (a, b) =>
            //{
            //    PkgUploader.DisableUpload();
            //    _pkgPath = b;
            //    await VerifyPackage();
            //};

            PkgChecker.PackageVerified += (a, pkg)
                => PkgUploader.EnableUpload(pkg);

            PkgUploader.UploadFinished += (a, r) =>
            {
                Alerter.Show(r, "Package Upload");
                if (r.IsSuccessful)
                    PkgChecker.CheckPackageCmd.ExecuteIfItCan();
            };
        }



        public AccessCheckerVM          AccessChecker  { get; private set; }
        public PackageCheckerVM         PkgChecker     { get; private set; }
        public PackageUploaderVM        PkgUploader    { get; private set; }
        public PreviousVersionsPopupVM  Previous       { get; private set; }
        public string                   ClientStatus   { get; set; }
        public string                   Title          { get; set; } = "Upload Package";
    }
}
