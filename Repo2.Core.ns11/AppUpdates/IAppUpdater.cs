using Repo2.Core.ns11.ChangeNotification;
using System;

namespace Repo2.Core.ns11.AppUpdates
{
    public interface IAppUpdater : IStatusChanger
    {
        event EventHandler UpdatesInstalled;

        void StartCheckingForUpdates();
    }
}
