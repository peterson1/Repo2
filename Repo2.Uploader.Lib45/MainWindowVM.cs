using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using PropertyChanged;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.InputCommands;
using Repo2.Uploader.Lib45.Configuration;
using Repo2.Uploader.Lib45.PackageFinders;

namespace Repo2.Uploader.Lib45
{
    [ImplementPropertyChanged]
    public class MainWindowVM
    {
        private IR2RestClient       _client;
        private IR2PreUploadChecker _preCheckr;
        private IPackageUploader    _pkgUploadr;
        private R2Package           _pkg;

        public MainWindowVM(IR2RestClient restClient,
                            IR2PreUploadChecker preUploadChecker,
                            IPackageUploader packageUploader)
        {
            _client               = restClient;
            _preCheckr            = preUploadChecker;
            _pkgUploadr           = packageUploader;

            _pkgUploadr.StatusChanged += (s, e) 
                => UploaderStatus = e.Text;

            FillConfigKeysCmd     = CreateFillConfigKeysCmd();
            CheckCredentialsCmd   = CreateCheckCredentialsCmd();
            CheckUploadabilityCmd = CreateCheckUploadabilityCmd();
            UploadPackageCmd      = CreateUploadPackageCmd();

            FillConfigKeysCmd.ExecuteIfItCan();
        }


        public string  ConfigKey       { get; set; }
        public string  PackagePath     { get; set; }
        public bool    CanWrite        { get; private set; }
        public bool    IsUploadable    { get; private set; }
        public string  UploaderStatus  { get; private set; }
        public double  MaxPartSizeMB   { get; set; } = 0.5;

        public LocalConfigFile  Config  { get; private set; }

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
            Config = null;
            Config = LocalConfigFile.Parse(ConfigKey);
            if (Config == null) return;

            CanWrite = await _client.EnableWriteAccess(Config);

            CheckUploadabilityCmd.ExecuteIfItCan();
        }


        private async Task CheckUploadability()
        {
            IsUploadable = false;
            _pkg         = LocalR2Package.From(PackagePath);
            IsUploadable = await _preCheckr.IsUploadable(_pkg);

            _pkg.nid = _preCheckr.LastPackage.nid;

            UploadPackageCmd.CurrentLabel = IsUploadable 
                ? "Upload Package" : _preCheckr.ReasonWhyNot;
        }


        private async Task UploadPackage()
        {
            _pkgUploadr.MaxPartSizeMB = this.MaxPartSizeMB;
            var reply = await _pkgUploadr.Upload(_pkg);

            Alerter.Show(reply, "Package Upload");

            if (reply.IsSuccessful)
                CheckUploadabilityCmd.ExecuteIfItCan();
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
                               x => CanWrite && !PackagePath.IsBlank(),
                               "Check Uploadability");

        private IR2Command CreateUploadPackageCmd()
            => R2Command.Async(UploadPackage, 
                x => IsUploadable && (MaxPartSizeMB > 0),
                    "...");
    }
}
