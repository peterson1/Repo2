using System.Diagnostics;
using System.Windows;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.SDK.WPF45.FileSystems;

namespace Repo2.SDK.WPF45.Extensions.ApplicationExtensions
{
    public static class ProcessExtensions
    {
        public static void Relaunch(this Application app, string arguments = null)
        {
            var origExe = new FileSystemAccesor1().CurrentExeFile;

            if (arguments.IsBlank())
                Process.Start(origExe);
            else
                Process.Start(origExe, arguments);

            app.Shutdown();
        }
    }
}
