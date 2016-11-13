using System;
using System.Diagnostics;
using System.IO;

namespace Repo2.SDK.WPF45.Extensions.FileInfoExtensions
{
    public static class CommonFileInfoExtensions
    {
        public static string SHA1ForFile(this FileInfo fileInfo)
            => SHA1ForFile(fileInfo.FullName);


        public static string SHA1ForFile(this string filePath)
        {
            if (!File.Exists(filePath)) return null;
            var algo = new HashLib.Crypto.SHA1();
            var byts = File.ReadAllBytes(filePath);
            var hash = algo.ComputeBytes(byts);
            return hash.ToString().ToLower();
        }


        public static string FileVersion(this FileInfo fileInfo)
        {
            if (!fileInfo.Exists) return null;
            var ver = FileVersionInfo.GetVersionInfo(fileInfo.FullName);
            return ver.FileVersion;
        }


        public static string NamePartOnly(this FileInfo fileInfo)
            => Path.GetFileNameWithoutExtension(fileInfo.FullName);


        public static string Base64Content(this FileInfo fileInfo)
        {
            if (!fileInfo.Exists) return null;
            var byts = File.ReadAllBytes(fileInfo.FullName);
            return Convert.ToBase64String(byts);
        }
    }
}