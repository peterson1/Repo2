using System.Windows;
using Autofac.Core;
using Repo2.SDK.WPF45.ComponentRegistry;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;

namespace Repo2.TestClient.WPF45
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Alerter.CatchErrors(this);

            var win = new MainWindow();
            try
            {
                using (var scope = DownloaderIoC.BeginScope())
                    win.DataContext = new MainWindowVM(scope);

                win.Show();
            }
            catch (DependencyResolutionException ex)
            {
                Alerter.ShowError("Resolver Error", ex.GetMessage());
                win.Close();
            }
        }
    }
}
