using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.Extensions.StringExtensions
{
    public struct L
    {
        public static string f => Environment.NewLine;
        public static string F => L.f + L.f;
    }


    public static class BasicStringExtensions
    {


        public static string TextUpTo(this string text, string findThis)
        {
            var pos = text.IndexOf(findThis);
            if (pos == -1) return text;

            return text.Substring(0, pos + findThis.Length);
        }

        public static string TextBefore(this string text, string findThis)
        {
            var pos = text.IndexOf(findThis);
            if (pos == -1) return text;

            return text.Substring(0, pos);
        }

        public static string TextAfter(this string text, string findThis, bool seekFromEnd = false)
        {
            if (text == null || findThis == null) return null;
            var pos = seekFromEnd ? text.LastIndexOf(findThis)
                                  : text.IndexOf(findThis);
            if (pos == -1) return text;

            return text.Substring(pos + findThis.Length);
        }



        //public static string StripLineBreaks(this string text, string replacementText = " ")
        //    => text.IsBlank() ? text 
        //    : text.Replace("\r", replacementText)
        //          .Replace("\n", replacementText);

        // http://stackoverflow.com/a/8196219
        public static string StripLineBreaks(this string text, string replacementText = " ")
            => text.IsBlank() ? text
             : Regex.Replace(text, @"\r\n?|\n", replacementText);



        /// <summary>
        /// Returns true if string is null or whitespace.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsBlank(this string text)
        {
            if (text == null) return true;
            return string.IsNullOrWhiteSpace(text);
        }


        public static string ToTitle(this string text)
            => new string(CharsToTitleCase(text).ToArray());

        private static IEnumerable<char> CharsToTitleCase(string s)
        {
            bool newWord = true;
            foreach (char c in s)
            {
                if (newWord) { yield return Char.ToUpper(c); newWord = false; }
                else yield return Char.ToLower(c);
                if (c == ' ') newWord = true;
            }
        }


        public static string Between(this string fullText,
                    string firstString, string lastString,
                    bool seekLastStringFromEnd = false)
        {
            if (fullText.IsBlank()) return string.Empty;

            int pos1 = fullText.IndexOf(firstString) + firstString.Length;
            if (pos1 == -1) return fullText;

            int pos2 = seekLastStringFromEnd ?
                fullText.LastIndexOf(lastString)
                : fullText.IndexOf(lastString, pos1);
            if (pos2 == -1 || pos2 <= pos1) return fullText;

            return fullText.Substring(pos1, pos2 - pos1);
        }


        public static bool SameAs(this string text1, string text2, bool caseSensitive = false)
        {
            if (text1.IsBlank() && text2.IsBlank()) return true;
            if (text1.IsBlank() && !text2.IsBlank()) return false;
            if (!text1.IsBlank() && text2.IsBlank()) return false;

            if (caseSensitive)
                return text1.Trim() == text2.Trim();
            else
                return text1.Trim().ToLower()
                    == text2.Trim().ToLower();
        }



        public static bool IsNumeric(this string text)
        {
            if (text.IsBlank()) return false;
            text = text.Trim();
            text = text.TrimStart('-');
            text = text.Trim();

            var dots = text.CountOccurence('.');
            if (dots > 1) return false;
            if (dots == 1) text = text.Replace(".", "");

            foreach (char c in text.ToCharArray())
                if (!char.IsDigit(c)) return false;

            return true;
        }



        /// <summary>
        /// Counts occurences of a character in a string.
        /// </summary>
        /// <param name="fullText"></param>
        /// <param name="findThis">character to look for</param>
        /// <returns></returns>
        public static int CountOccurence(this string fullText, char findThis)
        {
            int count = 0;
            var chars = fullText.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
                if (chars[i] == findThis) count++;

            return count;
        }


        public static int ToInt(this string text)
        {
            int val; var ok = int.TryParse(text, out val);
            if (ok) return val;
            throw new FormatException($"Non-convertible to Int32: “{text}”.");
        }



        public static string TrimStart(this string fullStr, string subStr)
        {
            if (!fullStr.StartsWith(subStr)) return fullStr;
            return fullStr.Substring(subStr.Length);
        }


        /// <summary>
        /// Appends sub URL to base URL adding slashes as needed.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="between"></param>
        /// <returns></returns>
        public static string Slash
            (this string str1, object str2, string between = "/")
                => StringSandwich(str1, between, str2);

        public static string Bslash
            (this string str1, object str2, string between = @"\")
                =>  StringSandwich(str1, between, str2);

        private static string StringSandwich
            (string leftLoaf, string filling, object rightLoaf)
        {
            if (leftLoaf == null) leftLoaf = "";
            var s2 = (rightLoaf == null) ? "" : rightLoaf.ToString();
            if (!leftLoaf.EndsWith(filling)) leftLoaf += filling;
            if (s2.StartsWith(filling)) s2 = s2.TrimStart(filling);
            return leftLoaf + s2;
        }



        public static string Join(this IEnumerable<string> list, string delimeter)
            => string.Join(delimeter, list);



        public static string JoinNonBlanks(this string separator, params string[] args)
        {
            if (args.Length == 0) return string.Empty;
            var nonBlanks = args.Where(x => !x.IsBlank());
            if (nonBlanks.Count() == 0) return string.Empty;
            nonBlanks = nonBlanks.Select(x => x.Trim())
                                 .Where (x => !x.IsBlank());
            if (nonBlanks.Count() == 0) return string.Empty;
            return nonBlanks.Join(separator);
        }


        public static string SubstringFromEnd(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }


        public static List<string> SplitTrim(this string text, string separator)
        {
            var split = text.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return split.Select(x => x.Trim()).ToList();
        }
    }
}
