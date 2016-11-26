using System;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.PackageDownloaders
{
    public interface ILocalPackageFileUpdater
    {
        event EventHandler<string> TargetUpdated;

        void        SetTargetFile            (string targetFilePath);
        string      TargetPath               { get; }
        void        StartCheckingForUpdates  (TimeSpan checkInterval);
        void        StopCheckingForUpdates   ();
        Task<bool>  TargetIsOutdated         ();
        Task        UpdateTarget             ();
    }
}
