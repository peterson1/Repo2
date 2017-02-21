using System.Windows;
using System.Windows.Input;

namespace Repo2.SDK.WPF45.ControlBehaviors
{
    //from http://stackoverflow.com/a/20299923/3973863
    public static class KeyboardFocusBehavior
    {
        public static readonly DependencyProperty OnProperty;

        static KeyboardFocusBehavior()
        {
            OnProperty = DependencyProperty.RegisterAttached("On", typeof(FrameworkElement), typeof(KeyboardFocusBehavior), new PropertyMetadata(OnSetCallback));
        }


        public static void SetOn(UIElement element, FrameworkElement value)
        {
            element.SetValue(OnProperty, value);
        }

        public static FrameworkElement GetOn(UIElement element)
        {
            return (FrameworkElement)element.GetValue(OnProperty);
        }

        private static void OnSetCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var frameworkElement = (FrameworkElement)dependencyObject;
            var target = GetOn(frameworkElement);

            if (target == null)
                return;

            frameworkElement.Loaded += (s, e) => Keyboard.Focus(target);
        }
    }
}
