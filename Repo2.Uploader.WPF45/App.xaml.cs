using System.Windows;
using Autofac;
using Repo2.Uploader.Lib45;
using Repo2.Uploader.WPF45.Components;

namespace Repo2.Uploader.WPF45
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var win = new MainWindow();
            using (var scope = Registry.Build().BeginLifetimeScope())
            {
                win.DataContext = scope.Resolve<MainWindowVM>();
            }
            win.Show();
        }
    }
}
