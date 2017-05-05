using Repo2.Core.ns11.ChangeNotification;
using System;

namespace Repo2.SDK.WPF45.ChangeNotification
{
    public class StatusChangerN45 : StatusChanger
    {
        protected override void TryRemoveExisting(EventHandler<string> newHandler)
        {
            newHandler.TryRemoveFrom(ref _statusChanged);
        }
    }
}
