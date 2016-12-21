using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.SDK.WPF45.Converters
{
    public class VisibleIfNotBlankConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            return value.ToString().IsBlank() ? Visibility.Collapsed : Visibility.Visible;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
