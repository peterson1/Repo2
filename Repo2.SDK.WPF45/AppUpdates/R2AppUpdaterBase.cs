using Repo2.Core.ns11.AppUpdates;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.PackageDownloaders;
using Repo2.SDK.WPF45.ChangeNotification;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.Extensions.ApplicationExtensions;
using Repo2.SDK.WPF45.InputCommands;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Repo2.SDK.WPF45.AppUpdates
{
    public abstract class R2AppUpdaterBase : StatusChangerN45, IAppUpdater, INotifyPropertyChanged
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

            StatusChanged += (s, e) =>
            {
                Logs.Add(e);
                LogText = string.Join(L.f, Logs);
            };

            Logs.MaxCount = 20;

            UpdatesInstalled += (s, e) =>
            {
                OnUpdateInstalled?.Invoke();
                RelaunchCmd.OverrideEnabled = true;
                CommandManager.InvalidateRequerySuggested();
                if (AutoRelaunchOnUpdate) RelaunchCmd.ExecuteIfItCan();
            };
        }


        public IR2Command           RelaunchCmd           { get; }
        public string               LogText               { get; private set; }
        public Observables<string>  Logs                  { get; } = new Observables<string>();
        public bool                 AutoRelaunchOnUpdate  { get; set; }
        public Action               OnUpdateInstalled     { get; set; }


        protected abstract IR2Credentials GetCredentials();


        public void StartCheckingForUpdates(int? overrideIntervalSeconds = null)
        {
            if (_r2Upd8r.IsChecking) return;

            var creds = GetCredentials();
            if (creds == null) return;

            _r2Upd8r.SetCredentials(creds);

            var seconds = overrideIntervalSeconds ?? creds.CheckIntervalSeconds;
            var interval = TimeSpan.FromSeconds(seconds);
            _r2Upd8r.StartCheckingForUpdates(interval);
        }


        private void RelaunchApp() => Application.Current.Relaunch();
    }
}
