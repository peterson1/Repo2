using System;
using System.Windows;

namespace Repo2.SDK.WPF45.UserInputWindows
{
    //[AutoDependencyProperty]
    public partial class DialogueWindow1 : Window
    {
        public DialogueWindow1()
        {
            InitializeComponent();
        }


        public DateTime?  StartDate => _startDate.Value;
        public DateTime?  EndDate   => _endDate.Value;


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }


        internal void SetDates(DateTime startDate, DateTime endDate)
        {
            _startDate.Value = startDate;
            _endDate  .Value = endDate;
        }


        internal void SetLabels(string caption, string title, string subtitle, string buttonLabel)
        {
            this.Title     = $"   {caption}";
            _title.Text    = title;
            _subtitle.Text = subtitle;
            _okay.Content  = buttonLabel;
        }


        //internal void ShowProgess(string busyText)
        //{
        //    _busy.BusyContent = busyText;
        //    _busy.IsBusy = true;
        //    this.Show();
        //    this.Activate();
        //}
    }
}
