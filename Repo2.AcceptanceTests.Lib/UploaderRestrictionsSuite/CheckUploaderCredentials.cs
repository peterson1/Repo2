using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Extensions.BooleanExtensions;

namespace Repo2.AcceptanceTests.Lib.UploaderRestrictionsSuite
{
    public class CheckUploaderCredentials : R2Credentials
    {
        private IR2CredentialsChecker _chekr;

        public CheckUploaderCredentials(IR2CredentialsChecker credentialsChecker)
        {
            _chekr = credentialsChecker;
        }

        public string CanUpload()
        {
            _chekr.Check(this).Wait();
            return _chekr.CanWrite.ToYesNo();
        }
    }
}
