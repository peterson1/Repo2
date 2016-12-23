using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Repo2.SDK.WPF45.PrintTools
{
    public class PrintPreviewer
    {
        const double PPI = 96;

        public static void FitTo (double inchesWidth, double inchesHeight, params FrameworkElement[] frameworkElements)
        {
            var doc    = new FixedDocument();
            var width  = inchesWidth * PPI;
            var height = inchesHeight * PPI;
            //MessageBox.Show($"page count :  {frameworkElements.Length} sheets" + L.f
            //              + $"paper size :  {inchesWidth:N1} x {inchesHeight:N1} inches", 
            //              "   Print Preview Summary", 
            //                  MessageBoxButton.OK, MessageBoxImage.Information);

            doc.DocumentPaginator.PageSize = new Size(width, height);

            foreach (var ctrl in frameworkElements)
            {
                var scale = Math.Min(width / ctrl.ActualWidth,
                                     height / ctrl.ActualHeight);

                ctrl.LayoutTransform = new ScaleTransform(scale, scale);

                var size = new Size(width, height);
                ctrl.Measure(size);
                ((UIElement)ctrl).Arrange(new Rect(new Point(0, 0), size));
                var pg = new FixedPage();
                pg.Width = doc.DocumentPaginator.PageSize.Width;
                pg.Height = doc.DocumentPaginator.PageSize.Height;

                var ctxt = ctrl.DataContext;
                var panl = ctrl.Parent as Panel;
                if (panl == null)
                    throw new InvalidCastException($"Parent control of {ctrl.Name} must be a type of ‹Panel›.");

                panl.Children.Remove(ctrl);
                pg.Children.Add(ctrl);
                ctrl.DataContext = ctxt;

                var pgContent = new PageContent();
                pgContent.Child = pg;
                doc.Pages.Add(pgContent);
            }

            //var win = new PrintPreviewWindow1();
            //win.vwr.Document = doc;
            //win.Title = $"  Print Preview for {frameworkElements.Length} sheets on {inchesWidth:N1} x {inchesHeight:N1} inches";
            //win.Show();
            PrintPreviewWindow1.Show(doc, inchesWidth, inchesHeight, "UI elements");
        }
    }
}
