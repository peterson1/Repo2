using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.SDK.WPF45.Serialization;
using static System.Environment;

namespace Repo2.SDK.WPF45.Configuration
{
    public abstract class LocalConfigFile : R2Credentials
    {
        const SpecialFolder SPECIAL_DIR = SpecialFolder.LocalApplicationData;
        const string        EXTENSION   = "cfg";

        protected abstract string  SubFolder { get; }
        protected abstract string  Prefix    { get; }


        protected T Parse<T>(string cfgKey)
        {
            var fPath = GetFilePath(cfgKey);

            if (!File.Exists(fPath))
                throw new FileNotFoundException($"Missing local config file:{L.f}{fPath}{L.F}", fPath);

            var json = File.ReadAllText(fPath);
            return Json.Deserialize<T>(json);
        }


        protected string GetParentFolder()
            => Path.Combine(GetFolderPath(SPECIAL_DIR), SubFolder);

        protected string GetFileName(string cfgKey) 
            => $"{Prefix}{Sanitize(cfgKey)}.{EXTENSION}";


        private string Sanitize(string cfgKey)
        {
            var safeName = cfgKey;

            foreach (char c in Path.GetInvalidFileNameChars())
                safeName = safeName.Replace(c, '_');

            return safeName;
        }


        protected string GetFilePath(string cfgKey)
            => Path.Combine(GetParentFolder(), GetFileName(cfgKey));


        //protected bool Found(string cfgKey)
        //    => File.Exists(GetFilePath(cfgKey));


        protected IEnumerable<string> GetLocalKeys()
        {
            var dir   = GetParentFolder();
            var filtr = $"{Prefix}*.{EXTENSION}";
            var files = Directory.GetFiles(dir, filtr);
            return files.Select(x => ExtractKey(x));
        }


        private string ExtractKey(string filePath)
        {
            var nme = Path.GetFileName(filePath);
            return nme.Between(Prefix, $".{EXTENSION}");
        }
    }
}
