using System;
using System.Linq;
using System.Windows;
using Autofac;
using Autofac.Core;
using Repo2.Core.ns11.Exceptions;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
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
            try
            {
                win.DataContext = GetMainWindowVM(e);
                win.Show();
            }
            catch (DependencyResolutionException ex)
            {
                Alerter.ShowError("Resolver Error", ex.GetMessage());
            }
        }


        private MainWindowVM GetMainWindowVM(StartupEventArgs e)
        {
            MainWindowVM vm;
            using (var scope = Registry.Build().BeginLifetimeScope())
            {
                vm = scope.Resolve<MainWindowVM>();
                vm.PackagePath = e.Args.FirstOrDefault();
            }
            return vm;
        }
    }
}
