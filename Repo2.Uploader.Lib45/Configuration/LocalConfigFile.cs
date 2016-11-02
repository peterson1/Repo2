using System.IO;
using Repo2.Core.ns11.Authentication;
using Repo2.SDK.WPF45.Serialization;
using static System.Environment;

namespace Repo2.Uploader.Lib45.Configuration
{
    public class LocalConfigFile : R2Credentials
    {
        const SpecialFolder SPECIAL_DIR = SpecialFolder.LocalApplicationData;
        const string        SUBFOLDER   = @"Repo2\Uploader";


        public static LocalConfigFile Parse(string cfgKey)
        {
            var fPath = GetFilePath(cfgKey);

            if (!File.Exists(fPath))
                throw new FileNotFoundException("Missing local config file", fPath);

            var json = File.ReadAllText(fPath);
            return Json.Deserialize<LocalConfigFile>(json);
        }


        private static string GetParentFolder()
            => Path.Combine(GetFolderPath(SPECIAL_DIR), SUBFOLDER);

        private static string GetFileName(string cfgKey) 
            => $"Uploader_{cfgKey}.cfg";

        private static string GetFilePath(string cfgKey)
            => Path.Combine(GetParentFolder(), GetFileName(cfgKey));
    }
}
