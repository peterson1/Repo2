using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.Extensions.FileInfoExtensions;
using Repo2.SDK.WPF45.PackageFinders;

namespace Repo2.SDK.WPF45.FileSystems
{
    public class FileSystemAccesor1 : IFileSystemAccesor
    {
        public Task<bool> Delete(IEnumerable<string> filePaths)
            => Delete(filePaths.ToArray());


        public async Task<bool> Delete(params string[] filePaths)
        {
            foreach (var file in filePaths)
            {
                if (!File.Exists(file)) continue;
                await Task.Run(() =>
                {
                    try   { File.Delete(file); }
                    catch { }
                });
                if (File.Exists(file)) return false;
            }
            return true;
        }


        public async Task<bool> Move(string originalPath, string targetPath)
        {
            var deletd = await Delete(targetPath);
            if (!deletd) return false;

            await Task.Run(() =>
            {
                try   { File.Move(originalPath, targetPath); }
                catch { }
            });
            return File.Exists(targetPath);
        }


        public async Task<string> IsolateFile(R2Package localPkg)
        {
            var srcPath  = Chain(localPkg.LocalDir, localPkg.Filename);
            var destPath = Path.GetTempFileName();

            var task = Task.Run(() 
                => File.Copy(srcPath, destPath, true));

            await task;
            return destPath;
        }


        private string Chain(params string[] paths)
            => Path.Combine(paths);


        public string GetSHA1(string filePath)
            => filePath.SHA1ForFile();


        public string WriteTempFile(byte[] byts)
        {
            var path = Path.GetTempFileName();
            File.WriteAllBytes(path, byts);
            return path;
        }


        public string ReadBase64(string filePath)
            => Convert.ToBase64String(
                  File.ReadAllBytes(filePath));


        public string GetTempFilePath(string filename = null)
            => filename.IsBlank() ? Path.GetTempFileName()
                                  : Chain(TempDir, filename);


        public bool Found(string filePath)
            => File.Exists(filePath);



        public R2Package ToR2Package(string filePath)
            => LocalR2Package.From(filePath);


        public string TempDir 
            => Chain(Path.GetTempPath(), GetType().Name);
    }



    public class FileIO
    {
        public static void Delete(IEnumerable<string> filePaths)
            => new FileSystemAccesor1().Delete(filePaths);
    }
}
