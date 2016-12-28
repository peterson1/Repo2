using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.InputCommands;
using Repo2.Uploader.Lib45.Configuration;

namespace Repo2.Uploader.Lib45.UserControlVMs
{
    [ImplementPropertyChanged]
    public class ConfigCheckerVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged    = delegate { };
        public event EventHandler<string>        PackagePathChanged = delegate { };


        private SynchronizationContext _ui;
        private IR2RestClient          _client;


        public ConfigCheckerVM(IR2RestClient restClient)
        {
            _ui     = SynchronizationContext.Current;
            _client = restClient;
            CreateCommands();

            PropertyChanged += (a, b) =>
            {
                if (b.PropertyName == nameof(PackagePath) && CanWrite == true)
                    PackagePathChanged?.Invoke(a, PackagePath);
            };
        }


        public Observables<string>  ConfigKeys            { get; private set; } = new Observables<string>();
        public string               ConfigKey             { get; set; }
        public UploaderConfigFile   Config                { get; private set; }
        public bool?                CanWrite              { get; private set; }
        public Observables<string>  PackagePaths          { get; private set; } = new Observables<string>();
        public string               PackagePath           { get; set; }
        public IR2Command           FillConfigKeysCmd     { get; private set; }
        public IR2Command           CheckCredentialsCmd   { get; private set; }
        public IR2Command           StopCheckingCredsCmd  { get; private set; }

        public string PackageFileName => PackagePath?.TextAfter("\\", true);


        private void FillConfigKeys()
        {
            ConfigKey = null;
            AsUI(_ => ConfigKeys.Swap(UploaderConfigFile.GetKeys()));
            if (ConfigKeys.Any()) ConfigKey = ConfigKeys[0];
        }


        private async Task CheckCredentials()
        {
            Config   = null;
            CanWrite = null;
            PackagePaths.Clear();

            Config = UploaderConfigFile.Parse(ConfigKey);
            if (Config == null) return;

            CanWrite = await _client.EnableWriteAccess(Config);

            AsUI(_ => PackagePaths.Swap(Config.LocalPackages));
            if (PackagePaths.Any()) PackagePath = PackagePaths[0];
        }


        private void CreateCommands()
        {
            FillConfigKeysCmd = R2Command.Relay(FillConfigKeys,
                                null, "refresh config keys");

            CheckCredentialsCmd = R2Command.Async(CheckCredentials,
                              x => !ConfigKey.IsBlank(),
                                    "Check Credentials");

            StopCheckingCredsCmd = R2Command.Relay(_client.StopEnablingWriteAccess,
                               _ => _client.IsEnablingWriteAccess);
        }


        private void AsUI(SendOrPostCallback action)
            => _ui.Send(action, null);
    }
}
