using System.Windows;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.SDK.WPF45.Converters
{
    public abstract class ZeroToVisibleConverterBase : ValueConverterBase
    {
        protected bool IsZeroOrNull(object value)
        {
            if (value == null) return true;
            if (!value.ToString().IsNumeric()) return false;
            decimal? num = null; decimal tryNum;

            if (decimal.TryParse(value.ToString(), out tryNum))
                num = tryNum;

            if (!num.HasValue) return true;
            if (num.Value == 0) return true;
            return false;
        }
    }

    public class VisibleIfZeroConverter : ZeroToVisibleConverterBase
    {
        protected override object ConvertValue(object value)
            => IsZeroOrNull(value) ? Visibility.Visible : Visibility.Collapsed;
    }

    public class VisibleIfNotZeroConverter : ZeroToVisibleConverterBase
    {
        protected override object ConvertValue(object value)
            => IsZeroOrNull(value) ? Visibility.Collapsed : Visibility.Visible;
    }
}
