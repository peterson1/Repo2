using System;
using System.Windows;
using System.Windows.Controls;
using Repo2.SDK.WPF45.PrintTools;
using Repo2.SDK.WPF45.PrintTools.MatelichPaginators;

namespace Repo2.SDK.WPF45.Extensions.DataGridExtensions
{
    public static class DataGridPrintExtensions
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
    }
}
