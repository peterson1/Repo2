using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Extensions.BooleanExtensions;

namespace Repo2.AcceptanceTests.Lib.UploaderRestrictionsSuite
{
    public class CheckUploaderCredentials : R2Credentials
    {
        public string CanUpload()
        {
            //var ok = Loginator
            return false.ToYesNo().ToLower();
        }
    }
}
