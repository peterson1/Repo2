using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.Extensions.FileInfoExtensions;

namespace Repo2.SDK.WPF45.FileSystems
{
    public class FileSystemAccesor1 : IFileSystemAccesor
    {
        public Task Delete(IEnumerable<string> filePaths)
            => Delete(filePaths.ToArray());


        public Task Delete(params string[] filePaths)
            => new Task(() =>
            {
                foreach (var file in filePaths)
                    File.Delete(file);
            });


        public Task<string> IsolateFile(R2Package localPkg)
        {
            var srcPath  = Chain(localPkg.LocalDir, localPkg.Filename);
            var uniqName = $"{DateTime.Now.Ticks}.tmp";
            var destPath = Path.Combine(TempDir, uniqName);
            return new Task<string>(() =>
            {
                File.Copy(srcPath, destPath);
                return destPath;
            });
        }


        private string Chain(params string[] paths)
            => Path.Combine(paths);


        public string GetSHA1(string filePath)
            => filePath.SHA1ForFile();


        public string TempDir 
            => Chain(Path.GetTempPath(), GetType().Name);
    }
}
