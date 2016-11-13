using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.SDK.WPF45.InputCommands;
using Repo2.Uploader.Lib45.Configuration;

namespace Repo2.Uploader.Lib45
{
    [ImplementPropertyChanged]
    public class MainWindowVM
    {
        private IR2PackageChecker     _pkgCheckr;
        private IR2CredentialsChecker _credsCheckr;
        private LocalConfigFile       _cfg;

        public MainWindowVM(IR2CredentialsChecker credentialsChecker, IR2PackageChecker packageChecker)
        {
            _pkgCheckr          = packageChecker;
            _credsCheckr        = credentialsChecker;
            FillConfigKeysCmd   = R2Command.Relay(FillConfigKeys);

            CheckCredentialsCmd = R2Command.Async(CheckCredentials,
                              x => !ConfigKey.IsBlank());

            CheckRegistrationCmd = R2Command.Async(CheckRegistration,
                               x => _credsCheckr.CanWrite 
                                 && !PackagePath.IsBlank());

            UploadPackageCmd = R2Command.Async(UploadPackage, x => IsRegistered);

            FillConfigKeysCmd.ExecuteIfItCan();
        }


        public string  ConfigKey     { get; set; }
        public string  PackagePath   { get; set; }
        public bool    CanURead      { get; private set; }
        public bool    CanWrite      { get; private set; }
        public bool    IsRegistered  { get; private set; }

        public IR2Command  FillConfigKeysCmd     { get; private set; }
        public IR2Command  CheckCredentialsCmd   { get; private set; }
        public IR2Command  CheckRegistrationCmd  { get; private set; }
        public IR2Command  UploadPackageCmd      { get; private set; }

        public Observables<string> ConfigKeys { get; private set; } = new Observables<string>();


        private void FillConfigKeys()
        {
            ConfigKey = null;

            ConfigKeys.Swap(LocalConfigFile.GetKeys());

            if (ConfigKeys.Any())
                ConfigKey = ConfigKeys[0];

            CheckCredentialsCmd.ExecuteIfItCan();
        }


        private async Task CheckCredentials()
        {
            _cfg = null;
            _cfg = LocalConfigFile.Parse(ConfigKey);
            if (_cfg == null) return;

            await _credsCheckr.Check(_cfg);

            CanURead = _credsCheckr.CanRead;
            CanWrite = _credsCheckr.CanWrite;
            CheckRegistrationCmd.ExecuteIfItCan();
        }


        private async Task CheckRegistration()
        {
            IsRegistered = false;
            var fName = Path.GetFileName(PackagePath);
            IsRegistered = await _pkgCheckr.IsRegistered(fName, _cfg);
        }


        private Task UploadPackage()
        {
            throw new NotImplementedException();
        }
    }
}
