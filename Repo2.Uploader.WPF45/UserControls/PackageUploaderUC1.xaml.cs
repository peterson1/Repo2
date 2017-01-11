using System.Windows.Controls;

namespace Repo2.Uploader.WPF45.UserControls
{
    /// <summary>
    /// Interaction logic for PackageUploaderUC1.xaml
    /// </summary>
    public partial class PackageUploaderUC1 : UserControl
    {
        public PackageUploaderUC1()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                txtLog.Focus();
            };
        }
    }
}
