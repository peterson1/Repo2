using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.SDK.WPF45.Exceptions
{
    public class ThreadedAlerter
    {
        public static void CatchErrors(Application app, Action<string> errorLogger = null)
        {
            var msg = "";

            app.DispatcherUnhandledException += (s, e) =>
            {
                msg = Show(e.Exception, "Dispatcher");
                errorLogger?.Invoke(msg);
                e.Handled = true;
            };


            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                msg = Show(e.ExceptionObject, "CurrentDomain");
                errorLogger?.Invoke(msg);
            };


            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                msg = Show(e.Exception, "TaskScheduler");
                errorLogger?.Invoke(msg);
            };
        }


        public static string Show(object exceptionObj, string thrower)
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
            longMsg = ex.Info(true, true) + L.f + $"Final thrower :  ‹{thrower}›";

            PreExit:
            //Show($"Error from ‹{thrower}›", shortMsg);
            return longMsg;
        }


        public static void Show(string caption, string message, 
                                MessageBoxImage image = MessageBoxImage.Error, 
                                MessageBoxButton button = MessageBoxButton.OK)
        {
            new Thread(new ThreadStart(delegate
            {
                MessageBox.Show(message, $"   {caption}", button, image);
            }
            )).Start();
        }
    }
}
