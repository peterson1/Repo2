using System;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;

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


        private IRemotePackageManager _remote;
        private IFileSystemAccesor    _file;
        private IPackageDownloader    _downloadr;
        private bool                  _keepChecking;
        private R2Package             _remotePkg;


        public LocalPackageFileUpdater1(IRemotePackageManager remotePackageManager, 
                                        IFileSystemAccesor fileSystemAccesor,
                                        IPackageDownloader packageDownloader)
        {
            _remote    = remotePackageManager;
            _file      = fileSystemAccesor;
            _downloadr = packageDownloader;
            _downloadr . StatusChanged += (s, e) => SetStatus(e.Text);
        }


        public string    TargetPath     { get; private set; }


        public void StartCheckingForUpdates(TimeSpan checkInterval)
        {
            _keepChecking = true;
            while (_keepChecking)
            {

            }
        }


        public async Task<bool> TargetIsOutdated()
        {
            ValidateTargetFile();
            _remotePkg = null;

            var localPkg = _file.ToR2Package(TargetPath);
            var fName = localPkg.Filename;

            SetStatus($"Getting packages named “{fName}” ...");
            var list = await _remote.ListByFilename(fName);
            if (list.Count == 0) return false;

            if (list.Count > 1) throw Fault
                .NonSolo($"Server packages named “{fName}”", list.Count);

            _remotePkg = list.Single();
            return _remotePkg.Hash != localPkg.Hash;
        }


        public async Task UpdateTarget()
        {
            if (_remotePkg == null) throw Fault
                .BadCall(nameof(TargetIsOutdated), nameof(UpdateTarget));

            var unpackd = await _downloadr.DownloadAndUnpack
                                (_remotePkg, _file.TempDir);

            CheckHash(unpackd, "downloaded-unpacked package");

            await RetireCurrentPackage();

            await PromoteNewerPackage(unpackd);

            CheckHash(TargetPath, "downloaded-unpacked-placed package");

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

        private void ValidateTargetFile()
        {
            if (TargetPath.IsBlank()) throw Fault
                .BlankText("Target File Path");

            if (!_file.Found(TargetPath)) throw Fault
                .Missing("Target Local Package File", TargetPath);
        }


        private void RaiseTargetUpdated()
            => _targetUpdated?.Invoke(this, TargetPath);


        private void SetStatus(string text)
            => _statusChanged.Raise(text);


        public void SetTargetFile(string targetFilePath)
            => TargetPath = targetFilePath;


        public void StopCheckingForUpdates()
            => _keepChecking = false;
    }
}
