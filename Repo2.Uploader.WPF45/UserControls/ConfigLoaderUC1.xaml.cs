using System.Windows.Controls;
using Repo2.Uploader.Lib45.Configuration;

namespace Repo2.Uploader.WPF45.UserControls
{
    public partial class ConfigLoaderUC1 : UserControl
    {
        public ConfigLoaderUC1()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                VM?.FillConfigKeysCmd.ExecuteIfItCan();
            };
        }


        private ConfigLoaderVM VM => DataContext as ConfigLoaderVM;
    }
}
