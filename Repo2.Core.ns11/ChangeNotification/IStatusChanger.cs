using System;

namespace Repo2.Core.ns11.ChangeNotification
{
    public interface IStatusChanger
    {
        event EventHandler<string> StatusChanged;
        string Status { get; }
    }
}
