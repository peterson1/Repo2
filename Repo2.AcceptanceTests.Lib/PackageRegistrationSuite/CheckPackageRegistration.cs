using Repo2.Core.ns11.Extensions.BooleanExtensions;
using Repo2.Core.ns11.PackageRegistration;

namespace Repo2.AcceptanceTests.Lib.PackageRegistrationSuite
{
    public class CheckPackageRegistration
    {
        private IR2PreUploadChecker _checkr;

        public CheckPackageRegistration(IR2PreUploadChecker packageChecker)
        {
            _checkr = packageChecker;
        }


        public string PackageName { get; set; }


        public string IsRegistered()
            //=> _checkr.IsUploadable(PackageName).Result.ToYesNo();
            => _checkr.IsUploadable(null).Result.ToYesNo();
    }
}
