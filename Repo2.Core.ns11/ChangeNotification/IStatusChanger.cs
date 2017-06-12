using System;
using System.ComponentModel;

namespace Repo2.Core.ns11.ChangeNotification
{
    public interface IStatusChanger : INotifyPropertyChanged
    {
        event EventHandler<string> StatusChanged;
        string Status { get; }
    }
}
