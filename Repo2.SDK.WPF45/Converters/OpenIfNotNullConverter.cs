using System;
using System.Globalization;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;

namespace Repo2.SDK.WPF45.Converters
{
    public class OpenIfNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value == null ? WindowState.Closed : WindowState.Open);


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
