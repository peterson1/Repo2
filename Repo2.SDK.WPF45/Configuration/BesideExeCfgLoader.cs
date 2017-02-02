using System.IO;
using Repo2.Core.ns11.FileSystems;

namespace Repo2.SDK.WPF45.Configuration
{
    public class BesideExeCfgLoader<T> where T : class
    {
        const string SETTINGS_CFG = "settings.cfg";

        private IFileSystemAccesor _fs;
        private T _lastLoaded;

        public BesideExeCfgLoader(IFileSystemAccesor fileSystemAccesor)
        {
            _fs = fileSystemAccesor;
        }


        public T Load(T defaultCfg)
            => _lastLoaded ?? (_lastLoaded = ReadBesideExe(defaultCfg));


        private T ReadBesideExe(T defaultCfg)
        {
            var p = _fs.GetBesideExeFilePath(SETTINGS_CFG);

            try
            {
                return _fs.ReadJsonFileBesideExe<T>(SETTINGS_CFG);
            }
            catch (FileNotFoundException)
            {
                _fs.WriteJsonFileBesideExe(SETTINGS_CFG, defaultCfg);
                return defaultCfg;
            }
        }


        public void RewriteFile()
            => _fs.WriteJsonFileBesideExe(SETTINGS_CFG, _lastLoaded);
    }
}
