using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repo2.Core.ns11.Exceptions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Repo2.SDK.WPF45.Extensions.DataGridExtensions
{
    public static class DataGridColumnToggler
    {
        const string CTX_MENU_KEY = "DataGridColumnToggler_ctxMenu";

        public static string EnableToggledColumns (this DataGrid dg, DataGridLengthUnitType colWidthType = DataGridLengthUnitType.SizeToCells)
        {
            try
            {
                dg.ColumnHeaderStyle = CreateColumnHeaderStyle(dg, colWidthType);
                return "No errors";
            }
            catch (Exception ex)
            {
                return ex.Info(false, true);
            }
        }


        private static Style CreateColumnHeaderStyle(DataGrid dg, DataGridLengthUnitType colWidthType)
        {
            //DataGridColumnHeader hdr;
            //hdr.pad

            var colHdrStyle = new Style(typeof(DataGridColumnHeader));
            var ctxMenuKey  = $"ctxMenu_{Guid.NewGuid().ToString("N")}";
            var ctxMenu     = CreateContextMenu(dg, colWidthType);
            colHdrStyle.Resources.Add(ctxMenuKey, ctxMenu);

            colHdrStyle.Setters.Add(ContextMenuSetter(ctxMenu));
            colHdrStyle.Setters.Add(HorizontalAlignmentSetter);
            colHdrStyle.Setters.Add(FontWeightSetter);
            colHdrStyle.Setters.Add(PaddingSetter);

            return colHdrStyle;
        }


        private static Setter ContextMenuSetter(ContextMenu ctxMenu)
            => new Setter(DataGridColumnHeader.ContextMenuProperty, ctxMenu);


        private static Setter HorizontalAlignmentSetter
            => new Setter(DataGridColumnHeader.HorizontalContentAlignmentProperty, HorizontalAlignment.Center);


        private static Setter FontWeightSetter
            => new Setter(TextBlock.FontWeightProperty, FontWeights.Medium);


        private static Setter PaddingSetter
            => new Setter(DataGridColumnHeader.PaddingProperty, new Thickness(7, 5, 7, 5));


        private static ContextMenu CreateContextMenu(DataGrid dg, DataGridLengthUnitType colWidthType)
        {
            var cm = new ContextMenu();

            foreach (var col in dg.Columns)
            {
                var mnu = new MenuItem();
                mnu.Header = col.Header;
                mnu.IsCheckable = true;
                mnu.IsChecked = col.Visibility == Visibility.Visible;
                mnu.Click += (s, e) => col.Visibility = mnu.IsChecked
                            ? Visibility.Visible : Visibility.Collapsed;
                cm.Items.Add(mnu);
                col.Width = new DataGridLength(0, colWidthType);
            }

            return cm;
        }
    }
}
