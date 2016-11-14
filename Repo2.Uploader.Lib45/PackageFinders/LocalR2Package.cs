﻿using System.IO;
using Repo2.Core.ns11.DomainModels;
using Repo2.SDK.WPF45.Extensions.FileInfoExtensions;

namespace Repo2.Uploader.Lib45.PackageFinders
{
    public class LocalR2Package
    {
        public static R2Package From (string filePath)
        {
            var inf   = new FileInfo(filePath);
            if (!inf.Exists) return null;

            var pkg       = new R2Package(inf.Name);
            pkg.LocalDir  = inf.DirectoryName;
            pkg.LocalHash = inf.SHA1ForFile();
            return pkg;
        }
    }
}