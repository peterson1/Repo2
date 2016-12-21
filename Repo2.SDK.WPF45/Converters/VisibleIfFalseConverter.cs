using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Repo2.SDK.WPF45.Converters
{
    public class VisibleIfFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value == false ? Visibility.Visible : Visibility.Collapsed;


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
