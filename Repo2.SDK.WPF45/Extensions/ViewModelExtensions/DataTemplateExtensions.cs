using System.Windows;

namespace Repo2.SDK.WPF45.Extensions.ViewModelExtensions
{
    public static class DataTemplateExtensions
    {
        public static void SetTemplate<TData, TUiElement>(this Application app)
        {
            var dt = new DataTemplate(typeof(TData));
            dt.VisualTree = new FrameworkElementFactory(typeof(TUiElement));
            var key = new DataTemplateKey(typeof(TData));
            app.Resources.Add(key, dt);
        }
    }
}
