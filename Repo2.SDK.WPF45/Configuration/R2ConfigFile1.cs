using System.IO;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.SDK.WPF45.FileSystems;

namespace Repo2.SDK.WPF45.Configuration
{
    public class R2ConfigFile1 : DownloaderConfigFile
    {
        public new static R2ConfigFile1 Parse(string configKey)
        {
            var fs     = new FileSystemAccesor1();
            var cfg    = new R2ConfigFile1();
            var path   = cfg.GetFilePath(configKey);
            var cfgPwd = Saltify(configKey);
            return fs.DecryptJsonFile<R2ConfigFile1>(path, cfgPwd);
        }


        public static R2ConfigFile1 ParseOrDefault(string configKey,
            string username, string password, string baseUrl, string certificateThumb, int checkIntervalSeconds)
        {
            var path = new R2ConfigFile1().GetFilePath(configKey);
            if (File.Exists(path)) return Parse(configKey);

            var cfg = new R2ConfigFile1
            {
                Username             = username,
                Password             = password,
                BaseURL              = baseUrl,
                CertificateThumb     = certificateThumb,
                CheckIntervalSeconds = checkIntervalSeconds,
            };

            new FileSystemAccesor1()
                .EncryptJsonToFile(path, cfg, Saltify(configKey));

            return cfg;
        }


        private static string Saltify(string configKey)
        {
            var keyHash = configKey.SHA1ForUTF8();
            var salt1   = keyHash.Substring(0, 10);
            var salt2   = keyHash.Substring(12);
            return $"{salt1}{configKey}{salt2}".SHA1ForUTF8();
        }
    }
}
