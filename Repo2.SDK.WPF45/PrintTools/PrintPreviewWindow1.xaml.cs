using System.Windows;
using System.Windows.Controls;

namespace Repo2.SDK.WPF45.PrintTools
{
    public partial class PrintPreviewWindow1 : Window
    {
        public PrintPreviewWindow1()
        {
            InitializeComponent();
            Loaded += (a, b) =>
            {
                var cc = vwr.Template.FindName("PART_FindToolBarHost", vwr) as ContentControl;
                if (cc != null) cc.Visibility = Visibility.Collapsed;
            };
        }
    }
}
