using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;

namespace Repo2.AcceptanceTests.Lib.UploaderRestrictionsSuite
{
    public class TestCredentialsChecker : IR2CredentialsChecker
    {
        public bool  CanRead   { get; private set; }
        public bool  CanWrite  { get; private set; }


        public async Task Check(R2Credentials credentials)
        {
            await Task.Delay(1000 * 5);
        }
    }
}
