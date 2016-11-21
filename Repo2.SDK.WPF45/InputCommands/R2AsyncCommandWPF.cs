using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PropertyChanged;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.Exceptions;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.SDK.WPF45.InputCommands
{
    [ImplementPropertyChanged]
    public class R2AsyncCommandWPF : IR2Command
    {
        private   string            _origLabel;
        protected Func<Task>        _task;
        protected Predicate<object> _canExecute;


        internal R2AsyncCommandWPF(Func<Task> task, Predicate<object> canExecute, string buttonLabel)
        {
            _task        = task;
            _canExecute  = canExecute;
            CurrentLabel = buttonLabel;
        }


        public string    CurrentLabel      { get; set; }
        public bool      IsBusy            { get; protected set; }
        public bool      IsCheckable       { get; set; }
        public bool      IsChecked         { get; set; }
        public bool      OverrideEnabled   { get; set; } = true;
        public bool      DisableWhenDone   { get; set; }
        public bool      LastExecutedOK    { get; protected set; }
        public DateTime  LastExecuteStart  { get; protected set; }
        public DateTime  LastExecuteEnd    { get; protected set; }



        public async void Execute(object parameter)
        {
            if (IsBusy) return;
            if (!OverrideEnabled) return;

            IsBusy             = true;
            var origOverride   = OverrideEnabled;
            _origLabel         = CurrentLabel;
            CurrentLabel       = $"Running «{_origLabel}» ...{L.f}Please wait.";
            OverrideEnabled    = false;
            LastExecutedOK     = false;
            LastExecuteStart   = DateTime.Now;

            try
            {
                await _task.Invoke();
                LastExecutedOK = true;
            }
            catch (Exception ex)
            {
                //later: report error to server
                OnError(ex);
            }
            finally
            {
                LastExecuteEnd  = DateTime.Now;
                IsBusy          = false;
                CurrentLabel    = _origLabel;
                OverrideEnabled = DisableWhenDone ? false : origOverride;
                CommandManager.InvalidateRequerySuggested();
            }
        }


        protected virtual void OnError(Exception error)
            => Alerter.ShowError($"Error on task :  “{_origLabel}”", 
                                 error.Info(false, true));



        public event EventHandler CanExecuteChanged
        {
            add    { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public bool CanExecute(object parameter)
        {
            if (IsBusy) return false;
            if (!OverrideEnabled) return false;
            return _canExecute?.Invoke(parameter) ?? true;
        }


        public void ExecuteIfItCan(object param = null)
        {
            if (CanExecute(param)) Execute(param);
        }


        public override string ToString() => CurrentLabel;
    }
}
