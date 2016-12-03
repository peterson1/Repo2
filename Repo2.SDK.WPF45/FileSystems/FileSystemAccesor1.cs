using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.Encryption;
using Repo2.SDK.WPF45.Exceptions;
using Repo2.SDK.WPF45.Extensions.FileInfoExtensions;
using Repo2.SDK.WPF45.PackageFinders;
using Repo2.SDK.WPF45.Serialization;
using static System.Environment;

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


        public bool WriteRepo2LogFile(string fileNameSuffix, string content, bool raiseError)
        {
            try
            {
                var date = $"{DateTime.Now:yyyy-MM-dd_hhmm}";
                var fNme = $"{date}_{fileNameSuffix}.log";
                var envF = GetFolderPath(SpecialFolder.LocalApplicationData);
                var dir  = Chain(envF, "Repo2", "Logs");
                CreateDir(dir);
                File.WriteAllText(Chain(dir, fNme), content);
                return true;
            }
            catch (Exception ex)
            {
                if (raiseError) throw ex;
                return false;
            }
        }


        public T DecryptJsonFile<T>(string filePath, string decryptKey)
        {
            var raw  = File.ReadAllText(filePath);
            var json = AESThenHMAC.SimpleDecryptWithPassword(raw, decryptKey);
            return Json.Deserialize<T>(json);
        }


        public void EncryptJsonToFile<T>(string filePath, T obj, string encryptKey)
        {
            var json = Json.Serialize(obj);
            var raw  = AESThenHMAC.SimpleEncryptWithPassword(json, encryptKey);
            CreateDir(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, raw);
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


        //public void AppendTo(string filePath, string text)
        //    => File.AppendAllText(filePath, text);


        public void CreateDir(string foldrPath)
            => Directory.CreateDirectory(foldrPath);


        public string TempDir 
            => Chain(Path.GetTempPath(), GetType().Name);
    }



    public class FileIO
    {
        public static void Delete(IEnumerable<string> filePaths)
            => new FileSystemAccesor1().Delete(filePaths);
    }
}
