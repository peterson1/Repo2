using System.Windows;
using System.Linq;
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
                var vm = scope.Resolve<MainWindowVM>();
                vm.PackagePath = e.Args.FirstOrDefault();
                win.DataContext = vm;
            }
            win.Show();
        }
    }
}
