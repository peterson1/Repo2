using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.Compression;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.PackageDownloaders;
using Repo2.Core.ns11.PackageUploaders;

namespace Repo2.Uploader.Lib45.PackageUploaders
{
    [ImplementPropertyChanged]
    public class R2D8PackageUploader : IPackageUploader
    {
        private IFileSystemAccesor _fileIO;
        private IFileArchiver      _archivr;
        private IFileSplitter      _splitr;
        private IPartSender        _sendr;
        private IPackageManager    _pkgMgr;
        private IPackageDownloader _downloadr;

        public R2D8PackageUploader(IFileSystemAccesor fileSystemAccesor,
                                   IFileArchiver fileArchiver,
                                   IFileSplitter fileSplitter,
                                   IPartSender partSender,
                                   IPackageManager packageManager,
                                   IPackageDownloader packageDownloader)
        {
            _fileIO    = fileSystemAccesor;
            _archivr   = fileArchiver;
            _splitr    = fileSplitter;
            _sendr     = partSender;
            _pkgMgr    = packageManager;
            _downloadr = packageDownloader;
        }


        public double  MaxPartSizeMB  { get; set; }
        public string  Status         { get; private set; }


        public async Task Upload(R2Package localPkg)
        {
            var pkgPath = await _fileIO.IsolateFile(localPkg);

            await _archivr.CompressInPlace(pkgPath);

            var partPaths = await _splitr.Split(pkgPath, MaxPartSizeMB);
            await _fileIO.Delete(pkgPath);

            await _sendr.SendParts(partPaths, localPkg);
            await _fileIO.Delete(partPaths);

            await _pkgMgr.UpdateNode(localPkg);

            var downloadedPkgPath = await _downloadr
                .DownloadAndUnpack(localPkg, _fileIO.TempDir);

            var newHash = _fileIO.GetSHA1(downloadedPkgPath);
            await _fileIO.Delete(downloadedPkgPath);

            if (newHash != localPkg.LocalHash)
                throw Fault.HashMismatch("Original Package File", "Downloaded Package File");
        }
    }
}
