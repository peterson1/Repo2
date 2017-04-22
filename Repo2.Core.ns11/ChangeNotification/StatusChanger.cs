using System;

namespace Repo2.Core.ns11.ChangeNotification
{
    public class StatusChanger : IStatusChanger
    {
        private      EventHandler<string> _statusChanged;
        public event EventHandler<string>  StatusChanged
        {
            add    { _statusChanged -= value; _statusChanged += value; }
            remove { _statusChanged -= value; }
        }


        public string Status { get; private set; }



        protected virtual void SetStatus(string statusText)
            => _statusChanged?.Raise(Status = statusText);
    }
}
