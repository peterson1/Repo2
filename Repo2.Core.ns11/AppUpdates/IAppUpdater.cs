using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.InputCommands;
using System;

namespace Repo2.Core.ns11.AppUpdates
{
    public interface IAppUpdater : IStatusChanger
    {
        event EventHandler UpdatesInstalled;

        IR2Command           RelaunchCmd          { get; }
        string               LogText              { get; }
        Observables<string>  Logs                 { get; }
        bool                 AutoRelaunchOnUpdate { get; set; }

        void StartCheckingForUpdates(int? overrideIntervalSeconds = null);
    }
}
