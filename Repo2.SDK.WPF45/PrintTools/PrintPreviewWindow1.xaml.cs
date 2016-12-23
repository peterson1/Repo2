using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

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


        public static void Show(IDocumentPaginatorSource documentPages, double inchesWidth, double inchesHeight, string printJobDesc)
        {
            var win = new PrintPreviewWindow1();
            win.vwr.Document = documentPages;
            win.Title = $"  Print Preview for “{printJobDesc}”  ({documentPages?.DocumentPaginator?.PageCount} sheets on {inchesWidth:N1} x {inchesHeight:N1} inches)";
            win.Show();
        }
    }
}
