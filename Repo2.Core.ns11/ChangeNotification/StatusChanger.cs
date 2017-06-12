using System;
using System.ComponentModel;

namespace Repo2.Core.ns11.ChangeNotification
{
    public class StatusChanger : IStatusChanger, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected      EventHandler<string> _statusChanged;

        public event EventHandler<string>  StatusChanged
        {
            //add    { _statusChanged -= value; _statusChanged += value; }
            add
            {
                //if (_statusChanged == null || !_statusChanged.GetInvocationList().Contains(value))
                TryRemoveExisting(value);
                _statusChanged += value;
            }
            remove { _statusChanged -= value; }
        }

        public string Status { get; private set; }


        protected virtual void TryRemoveExisting(EventHandler<string> value)
        {
            _statusChanged -= value;
        }


        protected virtual void SetStatus(string statusText)
        {
            _statusChanged?.Raise(Status = statusText);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
        }


        //public int InvocatorsCount => _statusChanged?.GetInvocationList()?.Count() ?? 0;

        public void RaisePropertyChanged(object sender, PropertyChangedEventArgs e) 
            => PropertyChanged?.Invoke(sender, e);
    }
}
