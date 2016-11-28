using System;
using System.Threading.Tasks;
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



        public static void CatchErrors(Application app)
        {
            app.DispatcherUnhandledException += (s, e) =>
            {
                ShowError("Dispatcher Error", ToMsg(e.Exception));
                e.Handled = true;
            };


            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                ShowError("CurrentDomain Error", ToMsg(e.ExceptionObject));
            };


            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                ShowError("TaskScheduler Error", ToMsg(e.Exception));
            };
        }



        private static string ToMsg(object exceptionObj)
        {
            var shortMsg = ""; var longMsg = "";

            if (exceptionObj == null)
            {
                shortMsg = longMsg = $"NULL exception object received by global handler.";
                goto PreExit;
            }

            var ex = exceptionObj as Exception;
            if (ex == null)
            {
                shortMsg = longMsg = $"Non-exception object thrown: ‹{exceptionObj.GetType().Name}›";
                goto PreExit;
            }

            shortMsg = ex.Info(false, false);
            longMsg = ex.Info(true, true);// + L.f + $"Final thrower :  ‹{thrower}›";

            PreExit:
            //Show($"Error from ‹{thrower}›", shortMsg);
            return longMsg;
        }
    }
}
