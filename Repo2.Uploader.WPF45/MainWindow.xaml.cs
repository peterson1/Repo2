using System.Threading.Tasks;
using System.Windows;
using Repo2.Uploader.Lib45;
using Repo2.Uploader.WPF45.Popups;

namespace Repo2.Uploader.WPF45
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private MainWindowVM VM => DataContext as MainWindowVM;


        private void PreviousVersions_Click(object sender, RoutedEventArgs e)
        {
            //var win = new PreviousVersionsWindow1();
            //win.DataContext = VM.Previous;
            //win.Show();
        }
    }
}
