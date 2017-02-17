using System;
using System.Globalization;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;

namespace Repo2.SDK.WPF45.Converters
{
    public class OpenIfTrueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value ? WindowState.Open : WindowState.Closed;


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (WindowState)value == WindowState.Open;
    }
}
