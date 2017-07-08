using System.IO;
using Repo2.Core.ns11.FileSystems;

namespace Repo2.SDK.WPF45.Configuration
{
    public class BesideExeCfgLoader<T> where T : class
    {
        //const string cfgFilename = "settings.cfg";

        private IFileSystemAccesor _fs;
        private T _lastLoaded;

        public BesideExeCfgLoader(IFileSystemAccesor fileSystemAccesor)
        {
            _fs = fileSystemAccesor;
        }


        public T Load(T defaultCfg)
            => _lastLoaded ?? (_lastLoaded = ReadBesideExe(defaultCfg));


        private T ReadBesideExe(T defaultCfg, string cfgFilename = "settings.cfg")
        {
            var p = _fs.GetBesideExeFilePath(cfgFilename);

            try
            {
                return _fs.ReadJsonFileBesideExe<T>(cfgFilename);
            }
            catch (FileNotFoundException)
            {
                _fs.WriteJsonFileBesideExe(cfgFilename, defaultCfg);
                return defaultCfg;
            }
        }


        public void RewriteFile(string cfgFilename = "settings.cfg")
            => _fs.WriteJsonFileBesideExe(cfgFilename, _lastLoaded);
    }
}
