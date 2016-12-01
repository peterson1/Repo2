using System.Threading.Tasks;
using System.Windows;
using Repo2.Uploader.Lib45;

namespace Repo2.Uploader.WPF45
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += async (a, b) =>
            {
                await Task.Delay(500);

                ((MainWindowVM)DataContext)
                    .FillConfigKeysCmd.ExecuteIfItCan();
            };
        }
    }
}
