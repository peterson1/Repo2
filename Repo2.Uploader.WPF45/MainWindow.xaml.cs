using System.Windows;
using Repo2.Uploader.Lib45;

namespace Repo2.Uploader.WPF45
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private MainWindowVM VM => DataContext as MainWindowVM;
    }
}
