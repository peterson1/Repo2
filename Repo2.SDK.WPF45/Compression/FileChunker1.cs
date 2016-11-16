using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.SDK.WPF45.Compression
{
    // adapted from http://www.codeproject.com/Articles/1034347/Upload-large-files-to-MVC-WebAPI-using-partitionin
    //
    public class FileChunker1
    {
        const string PART_TOKEN = ".part_";


        public static bool IsPartOfMany(string filePath)
        {
            if (!filePath.Contains(PART_TOKEN)) return false;
            if (!filePath.TextAfter(".", true).IsNumeric()) return false;
            if (!filePath.Between(PART_TOKEN, ".", true).IsNumeric()) return false;
            return true;
        }


        public static List<string> Split(string filePath, string outputDir, double maxFileSizeMB)
        {
            const int READBUFFER_SIZE = 1024;

            var baseFName = Path.GetFileName(filePath);
            var chunkSize = Convert.ToInt32(maxFileSizeMB * 1024 * 1024);
            var readBuffr = new byte[READBUFFER_SIZE];
            var partsList = new List<string>();

            using (var fullStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var totalPartsCount = 0;
                if (fullStream.Length < chunkSize)
                    totalPartsCount = 1;
                else
                {
                    float preciseFileParts = ((float)fullStream.Length / (float)chunkSize);
                    totalPartsCount = (int)Math.Ceiling(preciseFileParts);
                }

                int partCount = 0;
                while (fullStream.Position < fullStream.Length)
                {
                    var partPath = Path.Combine(outputDir,
                        $"{baseFName}{PART_TOKEN}{partCount + 1}.{totalPartsCount}");

                    partsList.Add(partPath);

                    using (var partStream = new FileStream(partPath, FileMode.Create))
                    {
                        int bytesRemaining = chunkSize;
                        int bytesRead = 0;
                        while (bytesRemaining > 0 && (bytesRead = fullStream.Read(readBuffr, 0,
                         Math.Min(bytesRemaining, READBUFFER_SIZE))) > 0)
                        {
                            partStream.Write(readBuffr, 0, bytesRead);
                            bytesRemaining -= bytesRead;
                        }
                    }
                    partCount++;
                }

            }
            return partsList;
        }


        public static string Merge(string part1Path)
        {
            var baseFName = part1Path.Substring(0, part1Path.IndexOf(PART_TOKEN));
            var tailTokens = part1Path.Substring(part1Path.IndexOf(PART_TOKEN) + PART_TOKEN.Length);
            var pattern = Path.GetFileName(baseFName) + PART_TOKEN + "*";
            var matches = Directory.GetFiles(Path.GetDirectoryName(part1Path), pattern)
                                      .OrderBy(x => x).ToList();
            var mergeList = new List<SortedFile>();
            var fileIndex = 0; var fileCount = 0;

            if (!int.TryParse(tailTokens.Substring(0, tailTokens.IndexOf(".")), out fileIndex)
             || !int.TryParse(tailTokens.Substring(tailTokens.IndexOf(".") + 1), out fileCount))
                throw new FileFormatException("Invalid file part name: " + part1Path);

            if (matches.Count() != fileCount)
                throw new InvalidOperationException("Unexpected # of file parts");

            if (File.Exists(baseFName)) File.Delete(baseFName);

            foreach (string filePart in matches)
            {
                var sFile = new SortedFile(filePart);
                baseFName = filePart.Substring(0, filePart.IndexOf(PART_TOKEN));
                tailTokens = filePart.Substring(filePart.IndexOf(PART_TOKEN) + PART_TOKEN.Length);

                if (!int.TryParse(tailTokens.Substring(0, tailTokens.IndexOf(".")), out fileIndex))
                    throw new FileFormatException("Invalid file part name: " + filePart);

                sFile.FileOrder = fileIndex;
                mergeList.Add(sFile);
            }

            var orderedParts = mergeList.OrderBy(s => s.FileOrder)
                                        .Select(x => x.FileName).ToList();

            WriteOneBigFile(baseFName, orderedParts);
            return baseFName;
        }


        public static void WriteOneBigFile(string outputFilePath, IEnumerable<string> orderedParts)
        {
            using (var stream = new FileStream(outputFilePath, FileMode.Create))
            {
                foreach (var chunkPath in orderedParts)
                {
                    using (var fileChunk = new FileStream(chunkPath, FileMode.Open))
                    {
                        fileChunk.CopyTo(stream);
                    }
                }
            }
        }


        private struct SortedFile
        {
            public SortedFile(string fileName)
            {
                FileName = fileName;
                FileOrder = 0;
            }

            public String FileName { get; set; }
            public int FileOrder { get; set; }
        }

    }
}
