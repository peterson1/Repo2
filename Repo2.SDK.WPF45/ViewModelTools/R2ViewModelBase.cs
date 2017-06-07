using System.ComponentModel;
using Repo2.Core.ns11.ChangeNotification;
using System.Threading;
using PropertyChanged;
using System;
using System.Threading.Tasks;

namespace Repo2.SDK.WPF45.ViewModelTools
{
    //[ImplementPropertyChanged]
    public class R2ViewModelBase : INotifyPropertyChanged
    {
        private      EventHandler<string> _statusChanged;
        public event EventHandler<string>  StatusChanged
        {
            add    { _statusChanged -= value; _statusChanged += value; }
            remove { _statusChanged -= value; }
        }

        protected    PropertyChangedEventHandler _propertyChanged;
        public event PropertyChangedEventHandler  PropertyChanged
        {
            add    { _propertyChanged -= value; _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }

        protected    EventHandler _activateRequested;
        public event EventHandler  ActivateRequested
        {
            add    { _activateRequested -= value; _activateRequested += value; }
            remove { _activateRequested -= value; }
        }

        private SynchronizationContext _ui;


        public R2ViewModelBase()
        {
            _ui = SynchronizationContext.Current;
        }


        public string   Title     { get; private set; }
        public bool     IsBusy    { get; private set; }
        public string   BusyText  { get; private set; }
        public string   Status    { get; private set; }


        protected virtual void UpdateTitle(string text) => Title = text;


        protected void SetStatus(string status)
            => _statusChanged?.Raise(Status = status);


        protected void StartBeingBusy(string message)
        {
            AsUI(_ =>
            {
                IsBusy = true;
                BusyText = message;
            });
        }


        protected async Task StartBeingBusyAsync(string message)
        {
            await Task.Delay(1);
            StartBeingBusy(message);
            await Task.Delay(1);
        }

        protected void StopBeingBusy() => IsBusy = false;



        public void Activate() 
            => AsUI(_ => _activateRequested.Raise());


        public void AsUI(SendOrPostCallback action)
            => _ui.Send(action, null);


        protected void RaisePropertyChanged(string propertyName)
            => _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
