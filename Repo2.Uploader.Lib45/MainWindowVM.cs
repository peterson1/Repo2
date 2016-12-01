using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.PackageRegistration;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.InputCommands;
using Repo2.SDK.WPF45.PackageFinders;
using Repo2.Uploader.Lib45.Configuration;

namespace Repo2.Uploader.Lib45
{
    [ImplementPropertyChanged]
    public class MainWindowVM
    {
        private IR2RestClient          _client;
        private IR2PreUploadChecker    _preCheckr;
        private IPackageUploader       _pkgUploadr;
        private R2Package              _pkg;
        private SynchronizationContext _ui;

        public MainWindowVM(IR2RestClient restClient,
                            IR2PreUploadChecker preUploadChecker,
                            IPackageUploader packageUploader)
        {
            _client     = restClient;
            _preCheckr  = preUploadChecker;
            _pkgUploadr = packageUploader;
            _ui         = SynchronizationContext.Current;

            _pkgUploadr.StatusChanged += (s, statusText) 
                => UploaderStatus = statusText;

            _client.OnRetry += (s, e)
                => ClientStatus += $"{L.f}{e}";

            CreateCommands();
        }


        public string  ConfigKey       { get; set; }
        public string  PackagePath     { get; set; }
        public bool    CanWrite        { get; private set; }
        public bool    IsUploadable    { get; private set; }
        public string  UploaderStatus  { get; private set; }
        public string  ClientStatus    { get; private set; }
        public double  MaxPartSizeMB   { get; set; } = 0.5;

        public UploaderConfigFile  Config  { get; private set; }

        public IR2Command  FillConfigKeysCmd      { get; private set; }
        public IR2Command  CheckCredentialsCmd    { get; private set; }
        public IR2Command  CheckUploadabilityCmd  { get; private set; }
        public IR2Command  StartUploadCmd         { get; private set; }
        public IR2Command  StopUploadCmd          { get; private set; }

        public Observables<string> ConfigKeys   { get; private set; } = new Observables<string>();
        public Observables<string> PackagePaths { get; private set; } = new Observables<string>();


        private void FillConfigKeys()
        {
            ConfigKey = null;
            AsUI(_ => ConfigKeys.Swap(UploaderConfigFile.GetKeys()));
            if (ConfigKeys.Any()) ConfigKey = ConfigKeys[0];
        }


        private async Task CheckCredentials()
        {
            Config = null;
            Config = UploaderConfigFile.Parse(ConfigKey);
            if (Config == null) return;

            CanWrite = await _client.EnableWriteAccess(Config, new CancellationToken());

            AsUI(_ => PackagePaths.Swap(Config.LocalPackages));
            if (PackagePaths.Any()) PackagePath = PackagePaths[0];
        }


        private async Task CheckUploadability()
        {
            IsUploadable = false;
            _pkg         = LocalR2Package.From(PackagePath);
            IsUploadable = await _preCheckr.IsUploadable(_pkg, new CancellationToken());

            if (IsUploadable)
                _pkg.nid = _preCheckr.LastPackage.nid;

            StartUploadCmd.CurrentLabel = IsUploadable 
                ? "Upload Package" : _preCheckr.ReasonWhyNot;
        }


        private async Task StartUpload()
        {
            _pkgUploadr.MaxPartSizeMB = this.MaxPartSizeMB;
            var reply = await _pkgUploadr.StartUpload(_pkg);

            Alerter.Show(reply, "Package Upload");

            if (reply.IsSuccessful)
                CheckUploadabilityCmd.ExecuteIfItCan();
        }


        private void StopUpload()
        {
            _pkgUploadr.StopUpload();
            StartUploadCmd.ConcludeExecute();
        }


        private void CreateCommands()
        {
            FillConfigKeysCmd = R2Command.Relay(FillConfigKeys,
                                null, "refresh config keys");

            CheckCredentialsCmd = R2Command.Async(CheckCredentials,
                              x => !ConfigKey.IsBlank(),
                                    "Check Credentials");

            CheckUploadabilityCmd = R2Command.Async(CheckUploadability,
                                x => CanWrite && !PackagePath.IsBlank(),
                                    "Check Uploadability");

            StartUploadCmd = R2Command.Async(StartUpload,
                           x => IsUploadable && (MaxPartSizeMB > 0),
                                "...");

            StopUploadCmd = R2Command.Relay(StopUpload,
                        x => _pkgUploadr.IsUploading, "stop uploading");
        }


        private void AsUI(SendOrPostCallback action)
            => _ui.Send(action, null);
    }
}
