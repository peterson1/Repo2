using System.Linq;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.SDK.WPF45.InputCommands;
using Repo2.Uploader.Lib45.Configuration;
using Repo2.Uploader.Lib45.PackageFinders;

namespace Repo2.Uploader.Lib45
{
    [ImplementPropertyChanged]
    public class MainWindowVM
    {
        private IR2CredentialsChecker _credsCheckr;
        private IR2PreUploadChecker   _preCheckr;
        private IPackageUploader    _pkgUploadr;
        private LocalConfigFile       _cfg;
        private R2Package             _pkg;

        public MainWindowVM(IR2CredentialsChecker credentialsChecker, 
                            IR2PreUploadChecker preUploadChecker,
                            IPackageUploader packageUploader)
        {
            _credsCheckr          = credentialsChecker;
            _preCheckr            = preUploadChecker;
            _pkgUploadr           = packageUploader;
            FillConfigKeysCmd     = CreateFillConfigKeysCmd();
            CheckCredentialsCmd   = CreateCheckCredentialsCmd();
            CheckUploadabilityCmd = CreateCheckUploadabilityCmd();
            UploadPackageCmd      = CreateUploadPackageCmd();

            FillConfigKeysCmd.ExecuteIfItCan();
        }


        public string  ConfigKey     { get; set; }
        public string  PackagePath   { get; set; }
        public bool    CanURead      { get; private set; }
        public bool    CanWrite      { get; private set; }
        public bool    IsUploadable  { get; private set; }

        public IR2Command  FillConfigKeysCmd      { get; private set; }
        public IR2Command  CheckCredentialsCmd    { get; private set; }
        public IR2Command  CheckUploadabilityCmd  { get; private set; }
        public IR2Command  UploadPackageCmd       { get; private set; }

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
            CheckUploadabilityCmd.ExecuteIfItCan();
        }


        private async Task CheckUploadability()
        {
            IsUploadable = false;
            _pkg         = LocalR2Package.From(PackagePath);
            IsUploadable = await _preCheckr.IsUploadable(_pkg);
        }


        private async Task UploadPackage()
        {
            await _pkgUploadr.Upload(_pkg);
        }


        private IR2Command CreateFillConfigKeysCmd()
            => R2Command.Relay(FillConfigKeys,
                        null, "refresh config keys");

        private IR2Command CreateCheckCredentialsCmd()
            => R2Command.Async(CheckCredentials,
                              x => !ConfigKey.IsBlank(),
                              "Check Credentials");

        private IR2Command CreateCheckUploadabilityCmd()
            => R2Command.Async(CheckUploadability,
                               x => _credsCheckr.CanWrite && !PackagePath.IsBlank(),
                               "Check Package Registration");

        private IR2Command CreateUploadPackageCmd()
            => R2Command.Async(UploadPackage, x => IsUploadable);
    }
}
