using System;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.ChangeNotification;

namespace Repo2.Core.ns11.PackageDownloaders
{
    public interface ILocalPackageFileUpdater : IStatusChanger
    {
        event EventHandler<string> TargetUpdated;

        void        SetTargetFile            (string targetFilePath);
        string      TargetPath               { get; }
        bool        IsChecking               { get; }

        void        SetCredentials           (IR2Credentials credentials, bool addCertToWhiteList = true);
        void        StartCheckingForUpdates  (TimeSpan checkInterval);
        void        StopCheckingForUpdates   ();
        Task<bool>  TargetIsOutdated         (CancellationToken cancelTkn);
        Task        UpdateTarget             (CancellationToken cancelTkn);
    }
}
