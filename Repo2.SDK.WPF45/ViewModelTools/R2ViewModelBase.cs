using System.ComponentModel;
using System.Threading;
using PropertyChanged;

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

        private SynchronizationContext _ui;


        public R2ViewModelBase()
        {
            _ui = SynchronizationContext.Current;
        }


        public string Title { get; private set; }


        protected void UpdateTitle(string text) => Title = text;


        protected void AsUI(SendOrPostCallback action)
            => _ui.Send(action, null);


        protected void RaisePropertyChanged(string propertyName)
            => _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
