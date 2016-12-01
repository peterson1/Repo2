﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using Autofac;
using PropertyChanged;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.PackageDownloaders;
using Repo2.SDK.WPF45.Configuration;
using Repo2.SDK.WPF45.InputCommands;

namespace Repo2.TestClient.WPF45
{
    [ImplementPropertyChanged]
    class MainWindowVM
    {
        private ILocalPackageFileUpdater _upd8r;

        public MainWindowVM(ILifetimeScope scope)
        {
            InitializeUpdater(scope);
            CreateCommands();
            LoadConfigCmd.ExecuteIfItCan();
        }

        public string  Status           { get; private set; }
        public bool    IsChecking       { get; private set; }
        public int     SecondsInterval  { get; set; }
        public string  ConfigKey        { get; set; } = "testDownloader1";

        public IR2Command  LoadConfigCmd     { get; private set; }
        public IR2Command  StartCheckingCmd  { get; private set; }
        public IR2Command  StopCheckingCmd   { get; private set; }
        public IR2Command  RelaunchCmd       { get; private set; }


        private void InitializeUpdater(ILifetimeScope scope)
        {
            _upd8r = scope.Resolve<ILocalPackageFileUpdater>();
            _upd8r.SetTargetFile(Assembly.GetEntryAssembly().Location);
            _upd8r.StatusChanged += (s, statusText) =>
            {
                Status += $"{L.f}{statusText}";
                IsChecking = _upd8r.IsChecking;
            };

            _upd8r.TargetUpdated += (s, e) =>
            {
                Status += $"{L.F}[event] Target Updated.";

                RelaunchCmd = R2Command.Relay(RelaunchApp, 
                          x => true, "Relaunch Now");
            };
        }


        private void CreateCommands()
        {
            LoadConfigCmd = R2Command.Relay(LoadConfig,
                        x => !IsChecking, "Load Config");

            StartCheckingCmd = R2Command.Relay(
                          () => _upd8r.StartCheckingForUpdates
                                (TimeSpan.FromSeconds(SecondsInterval)),
                           x => !IsChecking, "Start Checking");

            StopCheckingCmd = R2Command.Relay(_upd8r.StopCheckingForUpdates,
                          x => IsChecking, "Stop Checking");
        }


        private void LoadConfig()
        {
            //var cfg = DownloaderConfigFile.Parse(ConfigKey);
            var cfg = R2ConfigFile1.ParseOrDefault(ConfigKey,
                "usr", "pwd", "url", "thumb", 4);

            _upd8r.SetCredentials(cfg);
            SecondsInterval = cfg.CheckIntervalSeconds;
            StartCheckingCmd.ExecuteIfItCan();
        }


        private void RelaunchApp(object cmdParam)
        {
            Process.Start(_upd8r.TargetPath);
            var win = cmdParam as Window;
            win?.Close();
        }
    }
}