using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.SDK.WPF45.Serialization;
using static System.Environment;

namespace Repo2.Uploader.Lib45.Configuration
{
    public class LocalConfigFile : R2Credentials
    {
        const SpecialFolder SPECIAL_DIR = SpecialFolder.LocalApplicationData;
        const string        SUBFOLDER   = @"Repo2\Uploader";
        const string        PREFIX      = "Uploader_";
        const string        EXTENSION   = "cfg";

        public static LocalConfigFile Parse(string cfgKey)
        {
            var fPath = GetFilePath(cfgKey);

            if (!File.Exists(fPath))
                throw new FileNotFoundException($"Missing local config file:{L.f}{fPath}{L.F}", fPath);

            var json = File.ReadAllText(fPath);
            return Json.Deserialize<LocalConfigFile>(json);
        }


        private static string GetParentFolder()
            => Path.Combine(GetFolderPath(SPECIAL_DIR), SUBFOLDER);

        private static string GetFileName(string cfgKey) 
            => $"{PREFIX}{cfgKey}.{EXTENSION}";

        private static string GetFilePath(string cfgKey)
            => Path.Combine(GetParentFolder(), GetFileName(cfgKey));


        internal static IEnumerable<string> GetKeys()
        {
            var dir   = LocalConfigFile.GetParentFolder();
            var filtr = $"{PREFIX}*.{EXTENSION}";
            var files = Directory.GetFiles(dir, filtr);
            return files.Select(x => ExtractKey(x));
        }


        private static string ExtractKey(string filePath)
        {
            var nme = Path.GetFileName(filePath);
            return nme.Between(PREFIX, $".{EXTENSION}");
        }
    }
}
