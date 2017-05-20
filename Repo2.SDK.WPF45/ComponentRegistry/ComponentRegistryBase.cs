using Autofac;
using Autofac.Core;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.Extensions.IOCExtensions;
using Repo2.SDK.WPF45.FileSystems;
using System;
using System.Windows;

namespace Repo2.SDK.WPF45.ComponentRegistry
{
    public abstract class ComponentRegistryBase<TCfg>
        where TCfg : class
    {
        protected abstract void   SetDataTemplates    (Application app);
        protected abstract TCfg   LoadSettingsFile    (IFileSystemAccesor fs);
        protected abstract void   RegisterComponents  (ContainerBuilder b);
        protected abstract object ResolveMainWindowVM (ILifetimeScope scope);


        public object CreateMainVM(Application app)
        {
            if (app != null)
            {
                //app.SetTemplate<ConnectionsTabVM, ConnectionsTabUI>();
                //app.SetTemplate<ActivityLogVM, ActivityLogUI>();
                AddResourceDictionaries(app);
                SetDataTemplates(app);
            }

            var containr = RegisterAllComponents();
            var scope    = containr.BeginLifetimeScope();
            return TryResolveMainWindowVM(scope);
        }


        private IContainer RegisterAllComponents()
        {
            var b = new ContainerBuilder();
            Repo2IoC.RegisterComponentsTo(ref b);

            RegisterComponents(b);

            var cfg = LoadSettingsFile(new FileSystemAccesor1());
            b.RegisterInstance(cfg).AsSelf();
            return b.Build();
        }


        private object TryResolveMainWindowVM(ILifetimeScope scope)
        {
            try
            {
                return ResolveMainWindowVM(scope);
            }
            catch (DependencyResolutionException ex)
            {
                Alerter.ShowError("Resolver Error", ex.GetMessage());
                return null;
            }
        }


        protected virtual void AddResourceDictionaries(Application app)
            => AddToMergedDictionaries(app,
                @"/Repo2.SDK.WPF45;component/Styles/BasicDatagridTheme1.xaml",
                @"/Repo2.SDK.WPF45;component/Styles/DatagridTheme2.xaml",
                @"/Repo2.SDK.WPF45;component/Styles/FontAwesomeButtonTheme1.xaml",
                @"/Repo2.SDK.WPF45;component/Styles/ConvertersSet1.xaml",
                @"/Repo2.SDK.WPF45;component/Styles/NonReloadingTabControlTheme1.xaml",
                @"/Repo2.SDK.WPF45;component/Styles/ButtonsTheme1.xaml",
                @"/Repo2.SDK.WPF45;component/Styles/TextBlockTheme1.xaml");


        private void AddToMergedDictionaries(Application app, params string[] sources)
        {
            foreach (var src in sources)
            {
                var uri = new Uri(src, UriKind.Relative);
                var res = new ResourceDictionary { Source = uri };
                app.Resources.MergedDictionaries.Add(res);
            }
        }
    }
}
