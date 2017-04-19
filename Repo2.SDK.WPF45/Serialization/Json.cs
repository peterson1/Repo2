using System;
using Repo2.Core.ns11.Extensions.StringExtensions;
using ServiceStack.Text;
using System.Text;

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


        public static string Serialize<T>(T obj)
            => JsonSerializer.SerializeToString(obj);


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

    }
}
