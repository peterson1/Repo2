using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Extensions.BooleanExtensions;

namespace Repo2.AcceptanceTests.Lib
{
    public class CheckUploaderCredentials : R2Credentials
    {
        private TestCredentialsChecker _chekr;

        public CheckUploaderCredentials(TestCredentialsChecker credentialsChecker)
        {
            _chekr = credentialsChecker;
        }

        public string CanUpload()
        {
            //var ok = Credentials
            return _chekr?.Check().ToYesNo().ToLower();
        }
    }
}
