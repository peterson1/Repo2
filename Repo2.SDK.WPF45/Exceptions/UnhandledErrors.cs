using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.NodeManagers;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.SDK.WPF45.Configuration;

namespace Repo2.SDK.WPF45.Exceptions
{
    class UnhandledErrors
    {
        internal static void CatchFor(Application app, string configKey)
        {
            app.DispatcherUnhandledException += async (s, e) =>
            {
                e.Handled = true;
                await HandleError("Dispatcher", e.Exception, configKey);
            };


            AppDomain.CurrentDomain.UnhandledException += async (s, e)
                => await HandleError("CurrentDomain", e.ExceptionObject, configKey);


            TaskScheduler.UnobservedTaskException += async (s, e) =>
            {
                e.SetObserved();
                await HandleError("TaskScheduler", e.Exception, configKey);
            };
        }


        private static async Task HandleError<T>(string caughtBy, T exceptionObj, string configKey)
        {
            try
            {
                var cfg = R2ConfigFile1.Parse(configKey);

                using (var scope = Repo2IoC.BeginScope())
                {
                    var cli = scope.Resolve<IR2RestClient>();
                    var svr = scope.Resolve<IErrorTicketManager>();
                    var tkt = R2ErrorTicket.From(exceptionObj);

                    var ok = await cli.EnableWriteAccess(cfg, new CancellationToken());
                    if (!ok) throw new Exception("Failed to enable write access.");

                    if (!(await svr.Post(tkt)))
                        Alerter.ShowError(caughtBy, tkt.Description);
                }
            }
            catch (Exception ex)
            {
                Alerter.Show(ex, "Failed to Handle Error");
            }
        }
    }
}
