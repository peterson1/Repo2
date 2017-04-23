using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.Encryption;
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


        public string WriteDesktopFile(string filename, string contents)
        {
            var path = GetDesktopFilePath(filename);
            WriteTextFile(contents, path);
            return path;
        }


        public string WriteFileBesideExe(string filename, string contents)
        {
            var path = GetBesideExeFilePath(filename);
            WriteTextFile(contents, path);
            return path;
        }


        public bool DesktopFileFound(string filename)
            => File.Exists(GetDesktopFilePath(filename));


        public string WriteDesktopJsonFile<T>(string filename, T objectToSerialize)
        {
            var json = Json.Serialize(objectToSerialize);
            return WriteDesktopFile(filename, json);
        }


        public string WriteJsonFileBesideExe<T>(string filename, T objectToSerialize)
        {
            var json = Json.Serialize(objectToSerialize);
            return WriteFileBesideExe(filename, json);
        }


        public T ReadDesktopJsonFile<T>(string filename)
        {
            var json = File.ReadAllText(GetDesktopFilePath(filename));
            return Json.Deserialize<T>(json);
        }

        public T ReadJsonFileBesideExe<T>(string filename)
        {
            var json = File.ReadAllText(GetBesideExeFilePath(filename));
            return Json.Deserialize<T>(json);
        }


        public bool WriteRepo2LogFile(string fileNameSuffix, string content, bool raiseError)
        {
            try
            {
                var date = $"{DateTime.Now:yyyy-MM-dd_hhmm}";
                var fNme = $"{date}_{fileNameSuffix}.log";
                var envF = GetFolderPath(SpecialFolder.LocalApplicationData);
                var dir  = Chain(envF, "Repo2", "Logs");
                WriteTextFile(content, Chain(dir, fNme));
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
            if (raw.Length == 0) return default(T);
            if (raw[0] == Char.Parse("{")) return Json.Deserialize<T>(raw);

            var decryptd = AESThenHMAC.SimpleDecryptWithPassword(raw, decryptKey);
            return Json.Deserialize<T>(decryptd);
        }


        public void EncryptJsonToFile<T>(string filePath, T obj, string encryptKey)
        {
            var json = Json.Serialize(obj);
            var raw  = AESThenHMAC.SimpleEncryptWithPassword(json, encryptKey);
            WriteTextFile(raw, filePath);
        }


        public void WriteJsonFile(object objectToSerialize, string filePath)
            => WriteTextFile(Json.Serialize(objectToSerialize), filePath);


        public void WriteTextFile(string text, string filePath)
        {
            CreateDir(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, text);
        }


        public string ReadBase64(string filePath)
            => Convert.ToBase64String(
                  File.ReadAllBytes(filePath));


        public bool Found(string filePath)
            => File.Exists(filePath);



        public R2Package ToR2Package(string filePath)
            => LocalR2Package.From(filePath);


        //public void AppendTo(string filePath, string text)
        //    => File.AppendAllText(filePath, text);


        public void CreateDir(string foldrPath)
            => Directory.CreateDirectory(foldrPath);




        public string GetTempFilePath(string filename = null)
            => filename.IsBlank() ? Path.GetTempFileName()
                                  : Chain(TempDir, filename);

        public string GetDesktopFilePath(string filename) 
            => Chain(DesktopDir, filename);

        public string GetBesideExeFilePath(string filename)
            => Chain(CurrentExeDir, filename);

        public string GetAppDataFilePath(string subFoldername, string filename, string parentDir)
            => Chain(AppDataDir, parentDir, subFoldername, filename);


        public string AppDataDir        => GetFolderPath(SpecialFolder.LocalApplicationData);
        public string DesktopDir        => GetFolderPath(SpecialFolder.DesktopDirectory);
        //public string BesideExeDir      => GetBesideExeFileDir();
        public string CurrentExeFile    => Assembly.GetEntryAssembly()?.Location;
        public string CurrentExeDir     => GetBesideExeFileDir();
        public string CurrentExeVersion => GetFileVersion(CurrentExeFile);

        private string GetBesideExeFileDir()
        {
            //Directory.GetParent(Assembly.GetEntryAssembly().Location).FullName;

            var dir = CurrentExeFile ?? AppDomain.CurrentDomain.BaseDirectory;

            return Directory.GetParent(dir).FullName;
        }

        public string TempDir      => Chain(Path.GetTempPath(), GetType().Name);



        private string GetFileVersion(string filePath)
            => FileVersionInfo.GetVersionInfo(filePath).FileVersion;
    }



    public class FileIO
    {
        public static void Delete(IEnumerable<string> filePaths)
            => new FileSystemAccesor1().Delete(filePaths);
    }
}
