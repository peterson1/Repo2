using PropertyChanged;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.InputCommands;
using Repo2.SDK.WPF45.InputCommands;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.ComponentModel;

namespace Repo2.SDK.WPF45.ViewModelTools
{
    //[ImplementPropertyChanged]
    public abstract class TabbedMainWindowBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private      EventHandler<int> _selectedTabChanged;
        public event EventHandler<int>  SelectedTabChanged
        {
            add    { _selectedTabChanged -= value; _selectedTabChanged += value; }
            remove { _selectedTabChanged -= value; }
        }

        protected string _exeVer;


        public TabbedMainWindowBase(IFileSystemAccesor fs)
        {
            _exeVer = fs.CurrentExeVersion;
            ExitCmd = R2Command.Async(ExitApp);
            AppendToCaption("...");

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedTabIndex))
                    _selectedTabChanged.Raise(SelectedTabIndex);
            };
        }
        protected abstract string   CaptionPrefix   { get; }

        public string       Caption           { get; protected set; }
        public int          SelectedTabIndex  { get; set; }
        public IR2Command   ExitCmd           { get; }
        public bool         IsBusy            { get; private set; }
        public string       BusyText          { get; private set; }


        public Observables<R2ViewModelBase> Tabs { get; } = new Observables<R2ViewModelBase>();


        protected void StartBeingBusy(string message)
        {
            IsBusy = true;
            BusyText = message;
        }

        protected void StopBeingBusy() => IsBusy = false;



        protected virtual void AddAsTab(R2ViewModelBase tabVM)
        {
            var tabIndx = Tabs.Count;

            tabVM.ActivateRequested += (s, e)
                => SelectedTabIndex = tabIndx;

            Tabs.Add(tabVM);
        }


        private async Task ExitApp()
        {
            await BeforeExitApp();
            Application.Current.Shutdown();
        }


        protected virtual async Task BeforeExitApp()
        {
            await Task.Delay(1);
        }


        protected virtual void AppendToCaption(string text)
            => Caption = $"{CaptionPrefix}  v.{_exeVer}  :  {text}";
    }
}
