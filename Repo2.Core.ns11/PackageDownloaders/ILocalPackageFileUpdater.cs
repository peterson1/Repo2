using System;
using System.Threading.Tasks;
using Repo2.Core.ns11.ChangeNotification;

namespace Repo2.Core.ns11.PackageDownloaders
{
    public interface ILocalPackageFileUpdater : IStatusChanger
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
