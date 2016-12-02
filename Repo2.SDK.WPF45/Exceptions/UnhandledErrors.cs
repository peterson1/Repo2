using System;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Repo2.Core.ns11.Exceptions;

namespace Repo2.SDK.WPF45.Exceptions
{
    class UnhandledErrors
    {
        internal static void CatchFor(Application app, ILifetimeScope scope)
        {
            app.DispatcherUnhandledException += (s, e) =>
            {
                Alerter.ShowError("Dispatcher Error", ToMsg(e.Exception));
                e.Handled = true;
            };


            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Alerter.ShowError("CurrentDomain Error", ToMsg(e.ExceptionObject));
            };


            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Alerter.ShowError("TaskScheduler Error", ToMsg(e.Exception));
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
