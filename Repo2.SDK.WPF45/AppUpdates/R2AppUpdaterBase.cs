﻿using Repo2.Core.ns11.AppUpdates;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.PackageDownloaders;
using Repo2.SDK.WPF45.ChangeNotification;
using Repo2.SDK.WPF45.Extensions.ApplicationExtensions;
using Repo2.SDK.WPF45.InputCommands;
using System;
using System.Windows;
using System.Windows.Input;

namespace Repo2.SDK.WPF45.AppUpdates
{
    public abstract class R2AppUpdaterBase : StatusChangerN45, IAppUpdater
    {
        private      EventHandler  _updatesInstalled;
        public event EventHandler   UpdatesInstalled
        {
            add    { _updatesInstalled -= value; _updatesInstalled += value; }
            remove { _updatesInstalled -= value; }
        }


        private ILocalPackageFileUpdater _r2Upd8r;
        private IFileSystemAccesor       _fs;


        public R2AppUpdaterBase(ILocalPackageFileUpdater localPackageFileUpdater,
                                IFileSystemAccesor fileSystemAccesor)
        {
            _fs      = fileSystemAccesor;
            _r2Upd8r = localPackageFileUpdater;

            _r2Upd8r.SetTargetFile(_fs.CurrentExeFile);
            _r2Upd8r.StatusChanged += (s, e) => SetStatus(e);
            _r2Upd8r.TargetUpdated += (s, e) => _updatesInstalled?.Raise();

            RelaunchCmd = R2Command.Relay(RelaunchApp);
            RelaunchCmd.OverrideEnabled = false;

            UpdatesInstalled += (s, e) =>
            {
                RelaunchCmd.OverrideEnabled = true;
                CommandManager.InvalidateRequerySuggested();
            };
        }


        public IR2Command RelaunchCmd { get; }


        protected abstract IR2Credentials GetCredentials();


        public void StartCheckingForUpdates()
        {
            if (_r2Upd8r.IsChecking) return;


            var creds = GetCredentials();
            if (creds == null) return;

            _r2Upd8r.SetCredentials(creds);

            var interval = TimeSpan.FromSeconds(creds.CheckIntervalSeconds);
            _r2Upd8r.StartCheckingForUpdates(interval);
        }


        private void RelaunchApp() => Application.Current.Relaunch();
    }
}