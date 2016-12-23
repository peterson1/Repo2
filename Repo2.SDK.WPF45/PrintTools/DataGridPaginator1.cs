using System;
using System.Windows;
using System.Windows.Controls;
using Repo2.SDK.WPF45.PrintTools.MatelichPaginators;

namespace Repo2.SDK.WPF45.PrintTools
{
    public static class DataGridPaginator1
    {
        const double PPI = 96;

        public static bool AskToPrint(this DataGrid grid, 
            string headerLeftText, string headerCenterText = null,
            string headerRightText = null, string footerCenterText = null, 
            string printJobDesc = "Print Document")
        {
            var dlg = new PrintDialog();
            dlg.UserPageRangeEnabled = true;
            if (dlg.ShowDialog() == false) return false;

            var uri = new Uri("/Repo2.SDK.WPF45;component/PrintTools/MatelichPaginators/DataGridPaginatorResourceDictionary1.xaml", UriKind.RelativeOrAbsolute);
            var rsrc = new ResourceDictionary { Source = uri };
            var dSrc = new MatelichDataGridPaginator(grid, dlg, headerLeftText, headerCenterText, headerRightText, footerCenterText, rsrc);


            if (dlg.PageRangeSelection == PageRangeSelection.AllPages)
                dlg.PrintDocument(dSrc, printJobDesc);
            else
            {
                var rnge = new PageRangeDocumentPaginator(dSrc, dlg.PageRange);
                dlg.PrintDocument(rnge, printJobDesc);
            }
            return true;
        }

        //public static void PrintTo(this DataGrid grid, double inchesWidth, double inchesHeight, string documentTitle)
        //{
        //    var pages = DataGridPaginator1.GetPages(grid, inchesWidth, inchesHeight, documentTitle);
        //    PrintPreviewWindow1.Show(pages, inchesWidth, inchesHeight, documentTitle);
        //}

        //private static IDocumentPaginatorSource GetPages(DataGrid grid, double inchesWidth, double inchesHeight, string documentTitle)
        //{
        //    var mrgn = new Thickness(40, 30, 40, 30);
        //    var size = new Size(inchesWidth * PPI, inchesHeight * PPI);
        //    var uri  = new Uri("/Repo2.SDK.WPF45;component/PrintTools/MatelichDataGridPaginator/DataGridPaginatorResourceDictionary1.xaml", UriKind.RelativeOrAbsolute);
        //    var rsrc = new ResourceDictionary { Source = uri };
        //    var dSrc = new MatelichDataGridPaginator.MatelichDataGridPaginator(grid, documentTitle, size, mrgn, true, rsrc);
        //    MessageBox.Show($"dSrc.PageCount = {dSrc.PageCount}");
        //    var rnge = new PageRange(1, dSrc.PageCount);
        //    var pges = new PageRangeDocumentPaginator(dSrc, rnge);
        //    return pges.Source;
        //}
    }
}
