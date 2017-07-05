using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.SDK.WPF45.Configuration;

namespace Repo2.SDK.WPF45.Exceptions
{
    public static class UnhandledErrors
    {
        private static string _cfgKey;
        private static bool   _addToWhiteList;
        private static bool   _showError;


        public static void CatchFor(Application app, string configKey, bool addCertToWhiteList = true, bool showError = false)
        {
            _cfgKey         = configKey;
            _addToWhiteList = addCertToWhiteList;
            _showError      = showError;

            app.DispatcherUnhandledException += async (s, e) =>
            {
                e.Handled = true;
                await HandleError("Dispatcher", e.Exception);
            };


            AppDomain.CurrentDomain.UnhandledException += async (s, e)
                => await HandleError("CurrentDomain", e.ExceptionObject);


            TaskScheduler.UnobservedTaskException += async (s, e) =>
            {
                e.SetObserved();
                await HandleError("TaskScheduler", e.Exception);
            };
        }


        private static async Task HandleError<T>(string caughtBy, T exceptionObj)
        {
            try
            {
                using (var scope = Repo2IoC.BeginScope())
                {
                    var tkt = R2ErrorTicket.From(exceptionObj);

                    if (!R2ConfigFile1.Found(_cfgKey))
                    {
                        Alerter.ShowError($"{caughtBy} Error", tkt.Description);
                        return;
                    }

                    var cfg = R2ConfigFile1.Parse(_cfgKey);
                    var cli = scope.Resolve<IR2RestClient>();
                    var svr = scope.Resolve<IErrorTicketManager>();

                    var ok = await cli.EnableWriteAccess(cfg, _addToWhiteList);
                    if (!ok) throw new Exception("Failed to enable write access.");

                    svr.Post(tkt);

                    if (_showError)
                        Alerter.ShowError($"{caughtBy} Error", tkt.Title);
                }
            }
            catch (Exception ex)
            {
                Alerter.Show(ex, "Failed to Handle Error");
            }
        }


        public static void OnUnhandledErrors(this Application app, Action<Exception> handler)
        {
            app.DispatcherUnhandledException += (s, e) =>
            {
                e.Handled = true;
                handler(e.Exception);
            };


            AppDomain.CurrentDomain.UnhandledException += (s, e)
                => handler(e.ExceptionObject as Exception);


            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                e.SetObserved();
                handler(e.Exception);
            };
        }
    }
}
