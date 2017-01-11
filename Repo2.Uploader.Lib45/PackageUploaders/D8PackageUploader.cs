using System;
using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.Compression;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageDownloaders;
using Repo2.Core.ns11.PackageUploaders;

namespace Repo2.Uploader.Lib45.PackageUploaders
{
    [ImplementPropertyChanged]
    public class D8PackageUploader : IPackageUploader
    {
        private      EventHandler<string> _statusChanged;
        public event EventHandler<string>  StatusChanged
        {
            add    { _statusChanged -= value; _statusChanged += value; }
            remove { _statusChanged -= value; }
        }

        private IFileSystemAccesor       _fileIO;
        private IFileArchiver            _archivr;
        private IPartSender              _sendr;
        private IRemotePackageManager    _pkgMgr;
        private IPackageDownloader       _downloadr;
        private CancellationTokenSource  _cancelr;


        public D8PackageUploader(IFileSystemAccesor fileSystemAccesor,
                                 IFileArchiver fileArchiver,
                                 IPartSender partSender,
                                 IRemotePackageManager packageManager,
                                 IPackageDownloader packageDownloader)
        {
            _fileIO    = fileSystemAccesor;
            _archivr   = fileArchiver;
            _sendr     = partSender;
            _pkgMgr    = packageManager;
            _downloadr = packageDownloader;

            _sendr.StatusChanged += (s, statusText) 
                => SetStatus(statusText);
        }


        public double MaxPartSizeMB { get; set; } = 0.5;

        public bool IsUploading => !_cancelr?.IsCancellationRequested ?? false;


        //public async Task<NodeReply> Upload(R2Package localPkg)
        //{
        //    try
        //    {
        //        Alerter.Show(await ExecuteUpload(localPkg), "Upload");
        //    }
        //    catch (Exception ex)
        //    {
        //        Alerter.ShowError("Upload Error", ex.Info(false, true));
        //    }
        //}

        public async Task<NodeReply> StartUpload (R2Package localPkg, string revisionLog)
        {
            if (localPkg.nid == 0) throw Fault
                .BadData(localPkg, "nid should NOT be zero");

            _cancelr = new CancellationTokenSource();
            try
            {
                return await ExecuteUpload(localPkg, revisionLog, _cancelr.Token);
            }
            catch (OperationCanceledException ex)
            {
                return NodeReply.Fail(ex);
            }
        }


        private async Task<NodeReply> ExecuteUpload(R2Package localPkg, string revisionLog, CancellationToken cancelTkn)
        {
            SetStatus("Isolating local package file...");
            var pkgPath = await _fileIO.IsolateFile(localPkg);

            SetStatus("Compressing and splitting into parts...");
            var partPaths = await _archivr.CompressAndSplit(pkgPath, MaxPartSizeMB);
            await _fileIO.Delete(pkgPath);

            await _sendr.SendParts(partPaths, localPkg, cancelTkn);
            await _fileIO.Delete(partPaths);

            var newHash = await TryDownloadAndGetHash(localPkg, cancelTkn);
            if (newHash != localPkg.Hash)
                throw Fault.HashMismatch("Original Package File", "Downloaded Package File");

            SetStatus("Updating package node ...");
            return await _pkgMgr.UpdateRemoteNode(localPkg, revisionLog, cancelTkn);
        }


        private async Task<string> TryDownloadAndGetHash(R2Package localPkg, CancellationToken cancelTkn)
        {
            SetStatus("Verifying uploaded parts ...");

            var downloadedPkgPath = await _downloadr
                .DownloadAndUnpack(localPkg, _fileIO.TempDir, cancelTkn);

            var newHash = _fileIO.GetSHA1(downloadedPkgPath);
            await _fileIO.Delete(downloadedPkgPath);
            return newHash;
        }


        public void StopUpload()
        {
            if (_cancelr == null) return;
            _cancelr.Cancel(true);
            SetStatus("Uploading stopped.");
        }


        private void SetStatus(string text) 
            => _statusChanged?.Invoke(this, text);
    }
}
