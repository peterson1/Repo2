using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Repo2.SDK.WPF45.Extensions.DataGridExtensions
{
    public static class DataGridColumnToggler
    {
        const string CTX_MENU_KEY = "DataGridColumnToggler_ctxMenu";

        public static void EnableToggledColumns (this DataGrid dg, DataGridLengthUnitType colWidthType = DataGridLengthUnitType.SizeToCells)
            => dg.ColumnHeaderStyle = CreateColumnHeaderStyle(dg, colWidthType);
        //{
        //    if (dg.ColumnHeaderStyle == null)
        //        dg.ColumnHeaderStyle = new Style(typeof(DataGridColumnHeader));

        //    CreateColumnHeaderStyle(dg, colWidthType);
        //}


        private static Style CreateColumnHeaderStyle(DataGrid dg, DataGridLengthUnitType colWidthType)
        {
            var colHdrStyle = new Style(typeof(DataGridColumnHeader), dg.ColumnHeaderStyle);
            var ctxMenuKey  = $"ctxMenu_{Guid.NewGuid().ToString("N")}";
            var ctxMenu     = CreateContextMenu(dg, colWidthType);
            colHdrStyle.Resources.Add(ctxMenuKey, ctxMenu);

            colHdrStyle.Setters.Add(ContextMenuSetter(ctxMenu));

            colHdrStyle.Setters.Add(HorizontalAlignmentSetter);
            //colHdrStyle.Setters.Add(FontWeightSetter);
            colHdrStyle.Setters.Add(FontStyleSetter);
            colHdrStyle.Setters.Add(ForegroundSetter);
            colHdrStyle.Setters.Add(PaddingSetter);

            return colHdrStyle;
        }


        private static Setter ContextMenuSetter(ContextMenu ctxMenu)
            => new Setter(DataGridColumnHeader.ContextMenuProperty, ctxMenu);


        private static Setter HorizontalAlignmentSetter
            => new Setter(DataGridColumnHeader.HorizontalContentAlignmentProperty, HorizontalAlignment.Center);


        private static Setter FontWeightSetter
            => new Setter(TextBlock.FontWeightProperty, FontWeights.Medium);


        private static Setter FontStyleSetter
            => new Setter(TextBlock.FontStyleProperty, FontStyles.Italic);


        private static Setter ForegroundSetter
            => new Setter(DataGridColumnHeader.ForegroundProperty, Brushes.Gray);


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
