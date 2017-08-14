using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Repo2.SDK.WPF45.Converters
{
    [ValueConversion(typeof(List<uint>), typeof(string))]
    public class UintListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a String");

            var array = ((List<uint>)value)?.ToArray();

            return array == null ? "null" : String.Join(", ", array);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
