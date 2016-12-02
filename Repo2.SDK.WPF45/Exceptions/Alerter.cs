using System;
using System.Windows;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Exceptions;

namespace Repo2.SDK.WPF45.Exceptions
{
    public class Alerter
    {
        public static void ShowError(string caption, string message)
            => Show(caption, message, MessageBoxImage.Error);


        //public static void ShowInfo(string caption, string message)
        //    => Show(caption, message, MessageBoxImage.Information);


        public static void Show(Reply reply, string caption)
        {
            var msg = reply.IsSuccessful 
                ? "Result: Successful" : reply.ErrorsText;

            Show(caption, msg, 
                reply.IsSuccessful ? MessageBoxImage.Information
                                   : MessageBoxImage.Error);
        }


        public static void Show(Exception ex, string caption)
            => ShowError(caption, ex.Info(false, true));


        private static void Show(string caption, string message,
                                 MessageBoxImage image,
                                 MessageBoxButton button = MessageBoxButton.OK)
            => MessageBox.Show(message, $"   {caption}", button, image);
    }
}
