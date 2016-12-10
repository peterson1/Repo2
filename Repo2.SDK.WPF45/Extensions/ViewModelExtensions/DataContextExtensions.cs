using System.Windows;

namespace Repo2.SDK.WPF45.Extensions.ViewModelExtensions
{
    public static class DataContextExtensions
    {
        public static void ShowOn<T> (this object dataContext)
            where T: Window, new()
        {
            var win = new T();
            win.DataContext = dataContext;
            win.Show();
        }
    }
}
