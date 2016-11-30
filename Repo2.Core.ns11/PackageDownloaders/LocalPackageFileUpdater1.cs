using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.RestClients;


namespace Repo2.Core.ns11.PackageDownloaders
{
    public class LocalPackageFileUpdater1 : ILocalPackageFileUpdater
    {
        private      EventHandler<string> _targetUpdated;
        public event EventHandler<string>  TargetUpdated
        {
            add    { _targetUpdated -= value; _targetUpdated += value; }
            remove { _targetUpdated -= value; }
        }

        private      EventHandler<StatusText> _statusChanged;
        public event EventHandler<StatusText>  StatusChanged
        {
            add    { _statusChanged -= value; _statusChanged += value; }
            remove { _statusChanged -= value; }
        }


        private IRemotePackageManager   _remote;
        private IFileSystemAccesor      _file;
        private IPackageDownloader      _downloadr;
        private IR2RestClient           _client;
        private R2Package               _remotePkg;
        private CancellationTokenSource _cancelr;


        public LocalPackageFileUpdater1(IRemotePackageManager remotePackageManager, 
                                        IFileSystemAccesor fileSystemAccesor,
                                        IPackageDownloader packageDownloader,
                                        IR2RestClient r2RestClient)
        {
            _remote    = remotePackageManager;
            _file      = fileSystemAccesor;
            _client    = r2RestClient;
            _client    . OnRetry += (s, e) => SetStatus(e);
            _downloadr = packageDownloader;
            _downloadr . StatusChanged += (s, e) => SetStatus(e.Text);
        }


        public string   TargetPath   { get; private set; }
        public bool     IsChecking   => !_cancelr?.IsCancellationRequested ?? false;


        public async void StartCheckingForUpdates(TimeSpan checkInterval)
        {
            if (!ValidateTargetFile()) return;

            _cancelr = new CancellationTokenSource();
            var tkn  = _cancelr.Token;

            SetStatus("Started checking for updates.");
            while (IsChecking)
            {
                SetStatus($"Delaying for {checkInterval.Seconds:n0} seconds ...");
                try
                {
                    await Task.Delay(checkInterval, tkn);
                    if (IsChecking) await CheckForUpdateOnce(tkn);

                }
                catch (OperationCanceledException) { }
            }
        }


        private async Task CheckForUpdateOnce(CancellationToken cancelTkn)
        {
            var hasUpd8 = false;
            try
            {
                hasUpd8 = await TargetIsOutdated(cancelTkn);
            }
            catch (Exception ex)
            {
                SetStatus(ex.Info());
                StopCheckingForUpdates();
                return;
            }
            if (!hasUpd8)
            {
                SetStatus("Target is up-to-date. Will check again later.");
                return;
            }

            SetStatus("Update found. Downloading ...");
            await UpdateTarget(cancelTkn);
        }


        public async Task<bool> TargetIsOutdated(CancellationToken cancelTkn)
        {
            if (!ValidateTargetFile()) return false;
            _remotePkg = null;

            var localPkg = _file.ToR2Package(TargetPath);
            var fName = localPkg.Filename;

            SetStatus($"Getting packages named “{fName}” ...");
            var list = await _remote.ListByFilename(fName, cancelTkn);
            if (list.Count == 0) return false;

            if (list.Count > 1) throw Fault
                .NonSolo($"Server packages named “{fName}”", list.Count);

            _remotePkg = list.Single();
            return _remotePkg.Hash != localPkg.Hash;
        }


        public async Task UpdateTarget(CancellationToken cancelTkn)
        {
            if (_remotePkg == null) throw Fault
                .BadCall(nameof(TargetIsOutdated), nameof(UpdateTarget));

            var unpackd = string.Empty;
            try
            {
                unpackd = await _downloadr.DownloadAndUnpack
                            (_remotePkg, _file.TempDir, cancelTkn);
            }
            catch (Exception ex) { SetStatus(ex.Info(false, true)); }

            if (unpackd.IsBlank()) return;
            CheckHash(unpackd, "downloaded-unpacked package");

            await RetireCurrentPackage();

            await PromoteNewerPackage(unpackd);

            CheckHash(TargetPath, "downloaded-unpacked-placed package");

            StopCheckingForUpdates();
            RaiseTargetUpdated();
        }

        private async Task RetireCurrentPackage()
        {
            var retirement = _file.GetTempFilePath();
            var success    = await _file.Move(TargetPath, retirement);
            if (!success) throw Fault.CantMove(TargetPath, retirement);
        }

        private async Task PromoteNewerPackage(string unpackdPath)
        {
            var success = await _file.Move(unpackdPath, TargetPath);
            if (!success) throw Fault.CantMove(unpackdPath, TargetPath);
        }


        private void CheckHash(string filePath, string fileDescription)
        {
            if (_file.GetSHA1(filePath) != _remotePkg.Hash) throw Fault
                .HashMismatch(fileDescription, "remote package");
        }


        private bool ValidateTargetFile()
        {
            if (TargetPath.IsBlank())
            {
                SetStatus(Fault.BlankText("Target File Path").Message);
                return false;
            }

            if (!_file.Found(TargetPath))
            {
                SetStatus(Fault.Missing("Target Local Package File", TargetPath).Message);
                return false;
            }

            return true;
        }


        private void RaiseTargetUpdated()
            => _targetUpdated?.Invoke(this, TargetPath);


        private void SetStatus(string text)
            => _statusChanged.Raise(text);


        public void SetTargetFile(string targetFilePath)
            => TargetPath = targetFilePath;


        public void StopCheckingForUpdates()
        {
            try   { _cancelr.Cancel(false); }
            catch { }
            SetStatus("Stopped checking for updates.");
        }


        public void SetCredentials(R2Credentials credentials, bool addCertToWhiteList)
        {
            SetStatus($"Using credentials for “{credentials.Username}”");
            _client.SetCredentials(credentials, addCertToWhiteList);
            //_client.OnRetry 
        }
    }
}
