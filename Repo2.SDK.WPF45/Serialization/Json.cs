using System;
using Repo2.Core.ns11.Extensions.StringExtensions;
using ServiceStack.Text;
using System.Text;
using System.Linq;
using MoreLinq;

namespace Repo2.SDK.WPF45.Serialization
{
    public class Json
    {
        public static T DeserializeB64<T>(string jsonUtf8Base64)
        {
            var byts = Convert.FromBase64String(jsonUtf8Base64);
            var json = Encoding.UTF8.GetString(byts);
            return Json.Deserialize<T>(json);
        }


        public static string SerializeToB64<T>(T obj)
        {
            var json = Json.Serialize(obj);
            var byts = Encoding.UTF8.GetBytes(json);
            return Convert.ToBase64String(byts);
        }


        public static T Deserialize<T>(string json)
            => JsonSerializer.DeserializeFromString<T>(json);


        public static string Serialize<T>(T obj, bool indented = false)
        {
            var json = JsonSerializer.SerializeToString(obj);
            return indented ? Prettify(json) : json;
        }


        public static bool TryDeserialize<T>(string json, out T obj)
        {
            try
            {
                obj = Deserialize<T>(json);
                return true;
            }
            catch
            {
                obj = default(T);
                return false;
            }
        }


        public static T DeserializeOrDefault<T>(string json)
        {
            var output = default(T);

            if (Json.TryDeserialize<T>(json, out output))
                return output;
            else
            {
                var msg = $"Failed to deserialize ‹{typeof(T).Name}› from json:{L.f}“{json}”";
                Console.WriteLine(msg);
                return default(T);
            }
        }


        public static string Prettify(string json)
        {
            const string INDENT_STRING = "    ";
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < json.Length; i++)
            {
                var ch = json[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && json[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }

    }
}
