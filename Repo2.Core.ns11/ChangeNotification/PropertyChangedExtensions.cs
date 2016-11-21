using System;
using System.ComponentModel;
using System.Reflection;

namespace Repo2.Core.ns11.ChangeNotification
{
    public static class PropertyChangedExtensions
    {
        public static void OnChange(this INotifyPropertyChanged src, string propertyName, Action<object> actionOnValue)
        {
            src.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == propertyName)
                    actionOnValue.Invoke(GetValue(src, propertyName));
            };
        }

        private static object GetValue(object src, string propertyName)
        {
            var prop = src.GetType().GetRuntimeProperty(propertyName);
            return prop?.GetValue(src);
        }
    }
}
