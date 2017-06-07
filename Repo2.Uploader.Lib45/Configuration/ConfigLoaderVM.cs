using System;
using System.Linq;
using PropertyChanged;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.InputCommands;
using Repo2.SDK.WPF45.InputCommands;
using Repo2.SDK.WPF45.PackageFinders;
using Repo2.SDK.WPF45.ViewModelTools;

namespace Repo2.Uploader.Lib45.Configuration
{
    //[ImplementPropertyChanged]
    public class ConfigLoaderVM : R2ViewModelBase
    {
        public event EventHandler<UploaderConfigFile> ConfigLoaded = delegate { };
        public event EventHandler<R2Package> PackageChanged = delegate { };


        public ConfigLoaderVM()
        {
            FillConfigKeysCmd = R2Command.Relay(FillConfigKeys);
            LoadConfigCmd     = R2Command.Relay(LoadConfig);

            this.OnChange(nameof(Package), p =>
            {
                if (Package != null)
                    PackageChanged.Raise(Package);
            });
        }

        public Observables<string>     ConfigKeys   { get; private set; } = new Observables<string>();
        public string                  ConfigKey    { get; set; }
        public Observables<R2Package>  Packages     { get; private set; } = new Observables<R2Package>();
        public R2Package               Package      { get; set; }

        public IR2Command   FillConfigKeysCmd  { get; private set; }
        public IR2Command   LoadConfigCmd      { get; private set; }


        private void FillConfigKeys()
        {
            ConfigKey = null;
            AsUI(_ => ConfigKeys.Swap(UploaderConfigFile.GetKeys()));
            if (ConfigKeys.Any())
                ConfigKey = ConfigKeys[0];
        }


        private void LoadConfig()
        {
            Packages.Clear();

            var cfg = UploaderConfigFile.Parse(ConfigKey);
            if (cfg == null) return;

            ConfigLoaded?.Raise(cfg);

            var pkgs = cfg.LocalPackages
                          .Select(p => LocalR2Package.From(p));

            AsUI(_ => Packages.Swap(pkgs));
            if (Packages.Any())
                Package = Packages[0];
        }
    }
}
