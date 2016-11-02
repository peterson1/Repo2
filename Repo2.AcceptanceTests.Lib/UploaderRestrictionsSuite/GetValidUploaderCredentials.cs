using Repo2.Uploader.Lib45.Configuration;

namespace Repo2.AcceptanceTests.Lib.UploaderRestrictionsSuite
{
    public class GetValidUploaderCredentials
    {
        private LocalConfigFile _cfg;

        public GetValidUploaderCredentials()
        {
            _cfg = LocalConfigFile.Parse(UploaderCfg.KEY);
        }

        public string ValidUsername() => _cfg.Username;
        public string ValidPassword() => _cfg.Password;
    }
}
