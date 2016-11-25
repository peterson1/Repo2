using System.IO;
using System.Text;
using Repo2.Core.ns11.Randomizers;

namespace Repo2.AcceptanceTests.Lib.TestTools
{
    public class CreateFile
    {
        public static string WithSizeMB(double fileSizeMB, string filename)
        {
            var sb   = new StringBuilder();
            var fke  = new FakeFactory();
            var path = Path.Combine(Path.GetTempPath(), filename);
            var size = (int)(fileSizeMB * 1024 * 1024);

            while (sb.Length < size)
                sb.AppendLine(fke.Text);

            File.WriteAllText(path, sb.ToString());
            return path;
        }

        //public static string WithSizeMB(double fileSizeMB, string filename)
        //{
        //    var fke  = new FakeFactory();
        //    var nme  = $"sampleFile.{fileSizeMB}MB";
        //    var tmp1 = Path.Combine(Path.GetTempPath(), nme);
        //    var tmp2 = Path.Combine(Path.GetTempPath(), filename);
        //    var size = (int)(fileSizeMB * 1024 * 1024);

        //    if (!File.Exists(tmp1))
        //    {
        //        var sb = new StringBuilder();

        //        while (sb.Length < size)
        //            sb.AppendLine(fke.Text);

        //        File.WriteAllText(tmp1, sb.ToString());
        //    }

        //    File.Copy(tmp1, tmp2, true);

        //    return tmp2;
        //}
    }
}
