using System.Windows;

namespace Repo2.SDK.WPF45.Exceptions
{
    public class Alerter
    {
        public static void ShowError(string caption, string message,
                                     MessageBoxImage image = MessageBoxImage.Error,
                                     MessageBoxButton button = MessageBoxButton.OK)
        {
            MessageBox.Show(message, $"   {caption}", button, image);
        }
    }
}
