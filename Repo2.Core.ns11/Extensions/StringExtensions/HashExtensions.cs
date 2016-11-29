using System.Text;

namespace Repo2.Core.ns11.Extensions.StringExtensions
{
    public static class HashExtensions
    {
        /// <summary>
        /// From: HashLib 2.1 (Dec 29, 2013) Stable
        /// http://hashlib.codeplex.com/
        /// </summary>
        /// <param name="utf8Text"></param>
        /// <returns></returns>
        public static string SHA1ForUTF8(this string utf8Text)
        {
            if (utf8Text == null) utf8Text = "";
            var algo = new HashLib.Crypto.SHA1();
            var res = algo.ComputeString(utf8Text, Encoding.UTF8);
            return res.ToString().ToLower();
        }
    }
}
