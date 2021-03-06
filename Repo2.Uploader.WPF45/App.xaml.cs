﻿using System.Windows;
using Autofac;
using Autofac.Core;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.Extensions.ViewModelExtensions;
using Repo2.Uploader.Lib45;
using Repo2.Uploader.Lib45.Components;
using Repo2.Uploader.Lib45.MainTabVMs;
using Repo2.Uploader.WPF45.MainTabs;

namespace Repo2.Uploader.WPF45
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetDataTemplates();

            var win = new MainWindow();
            try
            {
                win.DataContext = GetMainWindowVM(e);
                win.Show();
            }
            catch (DependencyResolutionException ex)
            {
                Alerter.ShowError("Resolver Error", ex.GetMessage());
                win.Close();
            }
        }


        private MainWindowVM GetMainWindowVM(StartupEventArgs e)
        {
            MainWindowVM vm;
            using (var scope = UploaderIoC.BeginScope())
            {
                vm = scope.Resolve<MainWindowVM>();
                //vm.PackagePath = e.Args.FirstOrDefault();
            }
            return vm;
        }


        private void SetDataTemplates()
        {
            this.SetTemplate<UploaderTabVM, UploaderTab1>();
            this.SetTemplate<PreviousVerTabVM, PreviousVerTab1>();
        }
    }
}
