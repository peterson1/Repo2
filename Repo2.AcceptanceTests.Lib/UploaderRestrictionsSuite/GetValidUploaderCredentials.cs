using Repo2.Core.ns11.Authentication;

namespace Repo2.AcceptanceTests.Lib.UploaderRestrictionsSuite
{
    public class GetValidUploaderCredentials
    {
        private R2Credentials _creds;

        public GetValidUploaderCredentials(R2Credentials r2Credentials)
        {
            _creds = r2Credentials;
        }

        public string ValidUsername() => _creds.Username;
        public string ValidPassword() => _creds.Password;
    }
}
