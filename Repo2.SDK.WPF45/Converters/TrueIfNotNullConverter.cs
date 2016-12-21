using System;
using System.Globalization;
using System.Windows.Data;

namespace Repo2.SDK.WPF45.Converters
{
    public class TrueIfNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value != null;


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
