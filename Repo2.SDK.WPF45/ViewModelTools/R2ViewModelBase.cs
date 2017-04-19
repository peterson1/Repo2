using System.ComponentModel;
using Repo2.Core.ns11.ChangeNotification;
using System.Threading;
using PropertyChanged;
using System;

namespace Repo2.SDK.WPF45.ViewModelTools
{
    [ImplementPropertyChanged]
    public class R2ViewModelBase : INotifyPropertyChanged
    {
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


        public string Title { get; private set; }


        protected void UpdateTitle(string text) => Title = text;


        public void Activate() 
            => AsUI(_ => _activateRequested.Raise());


        protected void AsUI(SendOrPostCallback action)
            => _ui.Send(action, null);


        protected void RaisePropertyChanged(string propertyName)
            => _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
