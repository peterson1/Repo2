using System;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.RestClients;

namespace Repo2.Core.ns11.PackageDownloaders
{
    public abstract class LocalPackageFileUpdaterBase : ILocalPackageFileUpdater
    {
        public event EventHandler<string> TargetUpdated;


        private IRemotePackageManager _remote;
        private IFileSystemAccesor    _fileIO;
        private bool                  _keepChecking;


        public LocalPackageFileUpdaterBase(IRemotePackageManager remotePackageManager, IFileSystemAccesor fileSystemAccesor)
        {
            _remote = remotePackageManager;
            _fileIO = fileSystemAccesor;
        }


        public abstract R2Package R2PackageFromFile(string targetPath);


        public void StartCheckingForUpdates(TimeSpan checkInterval)
        {
            _keepChecking = true;
        }


        public async Task<bool> TargetIsOutdated()
        {
            ValidateTargetFile();

            var pkg  = R2PackageFromFile(TargetPath);

            var list = await _remote.ListByFilename(pkg);
            if (list.Count == 0) return false;

            return true;
        }


        public Task<bool> UpdateTarget()
        {
            throw new NotImplementedException();
        }


        private void ValidateTargetFile()
        {
            if (TargetPath.IsBlank()) throw Fault
                .BlankText("Target File Path");

            if (!_fileIO.Found(TargetPath)) throw Fault
                .Missing("Target Local Package File", TargetPath);
        }


        public string    TargetPath     { get; private set; }

        public void SetTargetFile(string targetFilePath)
            => TargetPath = targetFilePath;


        public void StopCheckingForUpdates()
            => _keepChecking = false;
    }
}
