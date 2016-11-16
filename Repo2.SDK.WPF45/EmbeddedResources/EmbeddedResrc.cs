using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo2.SDK.WPF45.EmbeddedResources
{
    public class EmbeddedResrc
    {
        public static string ExtractToFile<T>(string resrcFileName, string resrcFolder, string targetDir)
        {
            Directory.CreateDirectory(targetDir);
            var targPath = Path.Combine(targetDir, resrcFileName);
            var assembly = typeof(T).Assembly;
            var rsrcPath = $"{assembly.GetName().Name}.{resrcFolder}.{resrcFileName}";

            using (var input = assembly.GetManifestResourceStream(rsrcPath))
            using (var fStream = File.Create(targPath, (int)input.Length))
            {
                var byts = new byte[input.Length];
                input.Read(byts, 0, byts.Length);
                fStream.Write(byts, 0, byts.Length);
            }

            return targPath;
        }
    }
}
