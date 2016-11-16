using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Repo2.Core.ns11.Exceptions;
using Repo2.SDK.WPF45.EmbeddedResources;
using SevenZip;
using static System.Environment;

namespace Repo2.SDK.WPF45.Compression
{
    public class SevenZipper2
    {
        const string COMPRESSOR_LIB = "7za.dll";
        const string EXTRACTOR_LIB  = "7zxa.dll";
        const string APPDATA_SUBDIR = @"Repo2\7zip";


        public static Task<IEnumerable<string>> CompressAndSplit
            (string filePath, double maxPartSizeMB)
        {
            var tcs   = new TaskCompletionSource<IEnumerable<string>>();
            var tmp7z = Path.GetTempFileName();
            var zpr   = GetUltra7z2Compressor();

            zpr.CompressionFinished += (s, e) =>
            {
                var parts = FileChunker1.Split(tmp7z, Path
                                    .GetTempPath(), maxPartSizeMB);
                File.Delete(tmp7z);
                tcs.SetResult(parts);
            };

            zpr.BeginCompressFiles(tmp7z, filePath);

            return tcs.Task;
        }


        public static async Task<IEnumerable<string>> DecompressFromSplit
            (IEnumerable<string> orderedPartsPaths, string outputDir)
        {
            var oneBigF = Path.GetTempFileName();
            FileChunker1.WriteOneBigFile(oneBigF, orderedPartsPaths);

            var list = await ExtractArchive(oneBigF, outputDir);

            File.Delete(oneBigF);

            return list;
        }


        private static async Task<IEnumerable<string>> ExtractArchive
            (string archivePath, string targetDir)
        {
            var zpr  = GetExtractor(archivePath);
            var tcs  = new TaskCompletionSource<IEnumerable<string>>();
            var list = new List<string>();

            zpr.FileExtractionFinished += (s, e)
                => list.Add(Chain(targetDir, e.FileInfo.FileName));

            zpr.ExtractionFinished += (s, e) => tcs.SetResult(list);

            zpr.ExtractArchive(targetDir);

            var contents = await tcs.Task;

            if (contents == null) throw 
                Fault.NullRef<List<string>>("Extracted paths list");

            if (contents.Count() == 0) throw
                Fault.NoMember("Archive did not contain any file.");

            return contents;
        }


        private static SevenZipExtractor GetExtractor(string archivePath)
        {
            var libPath = GetLibPath(EXTRACTOR_LIB);

            if (!File.Exists(libPath))
                EmbeddedResrc.ExtractToFile<SevenZipper2>
                    (EXTRACTOR_LIB, "Compression", LibDir);

            SevenZipCompressor.SetLibraryPath(libPath);

            return new SevenZipExtractor
                (archivePath, InArchiveFormat.SevenZip);
        }


        private static SevenZipCompressor GetUltra7z2Compressor()
        {
            var libPath = GetLibPath(COMPRESSOR_LIB);
            SevenZipCompressor.SetLibraryPath(libPath);

            var zpr               = new SevenZipCompressor();
            zpr.ArchiveFormat     = OutArchiveFormat.SevenZip;
            zpr.CompressionLevel  = CompressionLevel.Ultra;
            zpr.CompressionMethod = CompressionMethod.Lzma2;
            zpr.CompressionMode   = CompressionMode.Create;

            return zpr;
        }


        private static string LibDir
            => Chain(GetFolderPath(SpecialFolder
                .LocalApplicationData), APPDATA_SUBDIR);

        private static string GetLibPath(string fileName)
            => Chain(LibDir, fileName);


        private static string Chain(params string[] paths)
            => Path.Combine(paths);
    }
}
