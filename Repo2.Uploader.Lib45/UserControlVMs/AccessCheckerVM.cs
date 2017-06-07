using System;
using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.InputCommands;
using Repo2.SDK.WPF45.ViewModelTools;
using Repo2.Uploader.Lib45.Configuration;

namespace Repo2.Uploader.Lib45.UserControlVMs
{
    //[ImplementPropertyChanged]
    public class AccessCheckerVM : R2ViewModelBase
    {
        public event EventHandler WriteAccessEnabled = delegate { };

        private SynchronizationContext _ui;
        private IR2RestClient _client;


        public AccessCheckerVM(ConfigLoaderVM configLoaderVM, 
                               IR2RestClient restClient)
        {
            _ui     = SynchronizationContext.Current;
            _client = restClient;

            configLoaderVM.ConfigLoaded += (s, e) =>
            {
                Config = e;
                CheckCredentialsCmd.ExecuteIfItCan();
            };

            CreateCommands();
        }


        public UploaderConfigFile   Config                { get; private set; }
        public bool?                CanWrite              { get; private set; }
        public IR2Command           CheckCredentialsCmd   { get; private set; }
        public IR2Command           StopCheckingCredsCmd  { get; private set; }
        public bool                 IsChecking            { get; private set; }



        private async Task StartChecking()
        {
            IsChecking = true;
            CanWrite   = null;
            CanWrite   = await _client.EnableWriteAccess(Config);
            IsChecking = false;
            if (CanWrite == true) WriteAccessEnabled?.Raise();
        }


        private void CreateCommands()
        {

            CheckCredentialsCmd = R2Command.Async(StartChecking,
                              x => Config != null, "Check Credentials");

            StopCheckingCredsCmd = R2Command.Relay(() => 
            {
                IsChecking = false;
                _client.StopEnablingWriteAccess();
            },
            _ => _client.IsEnablingWriteAccess);

            //StopCheckingCredsCmd.DisableWhenDone = true;
        }
    }
}
