using System;
using System.Net;
using System.Windows;

namespace Repo2.DnsUpdater.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var hostEntry = Dns.GetHostEntry("google.com");


            foreach (var addr in hostEntry.AddressList)
            {
                Console.WriteLine(addr.ToString());
            }
        }
    }
}
