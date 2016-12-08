using System;
using System.Windows;

namespace Repo2.SDK.WPF45.ProgressWindows
{
    public partial class ProgressWindow1 : Window
    {
        public ProgressWindow1(string caption)
        {
            InitializeComponent();
            this.Title = caption;
            this.ShowActivated = true;
        }

        public void Show(string busyText)
        {
            _busy.BusyContent = busyText;
            if (!this.IsVisible) this.Show();
            this.Activate();
        }
    }
}
