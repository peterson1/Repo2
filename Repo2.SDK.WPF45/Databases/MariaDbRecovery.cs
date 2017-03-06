using System;
using System.IO;
using System.Threading.Tasks;
using Repo2.Core.ns11.Exceptions;

namespace Repo2.SDK.WPF45.Databases
{
    public class MariaDbRecovery
    {
        const string FLAG_KEY   = "innodb_force_recovery";
        const string MY_INI_URI = @"core\mysql\my.ini";
        const string START_BAT  = @"scripts\start_mysql.bat";
        const string STOP_BAT   = @"scripts\stop_mysql.bat";


        public static async Task ForceRecover(string uniserverBaseDir)
        {
            var iniPath = FindMyIniFile(uniserverBaseDir);
            SetForceRecoveryFlag(1, iniPath);

            await Task.Delay(TimeSpan.FromSeconds(5));
        }


        private static string FindMyIniFile(string uniserverBaseDir)
        {
            var iniPath = Path.Combine(uniserverBaseDir, MY_INI_URI);

            if (!File.Exists(iniPath))
                throw Fault.Missing("MariaDB ini file", iniPath);

            return iniPath;
        }


        private static void SetForceRecoveryFlag(int forceRecoveryCode, string iniPath)
        {
            var lines = File.ReadAllLines(iniPath);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim().StartsWith(FLAG_KEY))
                    lines[i] = $"{FLAG_KEY} = {forceRecoveryCode}";
            }

            File.WriteAllLines(iniPath, lines);
        }
    }
}
