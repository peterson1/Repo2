using System;
using System.ComponentModel;
using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.Compression;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageDownloaders;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.SDK.WPF45.Exceptions;

namespace Repo2.Uploader.Lib45.PackageUploaders
{
    [ImplementPropertyChanged]
    public class D8PackageUploader : IPackageUploader
    {
        public event EventHandler<StatusText> StatusChanged;

        private IFileSystemAccesor _fileIO;
        private IFileArchiver      _archivr;
        private IPartSender        _sendr;
        private IRemotePackageManager    _pkgMgr;
        private IPackageDownloader _downloadr;


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

            _sendr.StatusChanged += (s, e) 
                => StatusChanged.Raise(e.Text);
        }


        public double MaxPartSizeMB { get; set; } = 0.5;


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

        public async Task<NodeReply> Upload(R2Package localPkg)
        {
            if (localPkg.nid == 0) throw Fault
                .BadData(localPkg, "nid should NOT be zero");

            StatusChanged.Raise("Isolating local package file...");
            var pkgPath = await _fileIO.IsolateFile(localPkg);

            StatusChanged.Raise("Compressing and splitting into parts...");
            var partPaths = await _archivr.CompressAndSplit(pkgPath, MaxPartSizeMB);
            await _fileIO.Delete(pkgPath);

            await _sendr.SendParts(partPaths, localPkg);
            await _fileIO.Delete(partPaths);

            var newHash = await TryDownloadAndGetHash(localPkg);
            if (newHash != localPkg.Hash)
                throw Fault.HashMismatch("Original Package File", "Downloaded Package File");

            StatusChanged.Raise("Updating package node ...");
            return await _pkgMgr.UpdateRemoteNode(localPkg);
        }

        private async Task<string> TryDownloadAndGetHash(R2Package localPkg)
        {
            var downloadedPkgPath = await _downloadr
                .DownloadAndUnpack(localPkg, _fileIO.TempDir);

            var newHash = _fileIO.GetSHA1(downloadedPkgPath);
            await _fileIO.Delete(downloadedPkgPath);
            return newHash;
        }
    }
}
