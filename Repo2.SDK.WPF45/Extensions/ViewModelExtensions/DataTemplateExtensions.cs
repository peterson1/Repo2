using System;
using System.Windows;

namespace Repo2.SDK.WPF45.Extensions.ViewModelExtensions
{
    public static class DataTemplateExtensions
    {
        public static void SetTemplate<TData, TUiElement>(this Application app)
            => app.SetTemplate(typeof(TData), typeof(TUiElement));
        //{
        //    var dt = new DataTemplate(typeof(TData));
        //    dt.VisualTree = new FrameworkElementFactory(typeof(TUiElement));
        //    var key = new DataTemplateKey(typeof(TData));
        //    app.Resources.Add(key, dt);
        //}


        public static void SetTemplate(this Application app, Type dataType, Type uiElementType)
        {
            var dt = new DataTemplate(dataType);
            dt.VisualTree = new FrameworkElementFactory(uiElementType);
            var key = new DataTemplateKey(dataType);
            app.Resources.Add(key, dt);
        }
    }
}
