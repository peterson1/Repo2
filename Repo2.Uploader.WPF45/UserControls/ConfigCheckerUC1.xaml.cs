using System.Windows.Controls;
using Repo2.Uploader.Lib45.UserControlVMs;

namespace Repo2.Uploader.WPF45.UserControls
{
    public partial class ConfigCheckerUC1 : UserControl
    {
        public ConfigCheckerUC1()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                VM.FillConfigKeysCmd.ExecuteIfItCan();
            };
        }

        private ConfigCheckerVM VM => DataContext as ConfigCheckerVM;
    }
}
