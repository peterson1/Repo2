using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.Core.ns11.Drupal8
{
    class D8HALJson
    {
        internal static Dictionary<string, object> GetNodeLinks<T>(T sourceObj, string baseUrl) where T : D8NodeBase
        {
            var dict = new Dictionary<string, object>();

            dict.Add("type", GetTypeHref(sourceObj, baseUrl));

            return dict;
        }

        internal static object GetFileLinks(string baseUrl)
        {
            var dict = new Dictionary<string, object>();

            dict.Add("type", WrapHref("rest/type/file/file", baseUrl));

            return dict;
        }


        private static Dictionary<string, object> GetTypeHref<T>(T sourceObj, string baseUrl) where T : D8NodeBase
            => WrapHref($"rest/type/node/{sourceObj.D8TypeName}", baseUrl);


        private static Dictionary<string, object> WrapHref(string value, string baseUrl)
            => new Dictionary<string, object> { { "href", baseUrl.Slash(value) } };


        internal static List<Dictionary<string, object>> ValueField<T>(T value,
            string key = "value") => WrapField(key, value);


        internal static List<Dictionary<string, object>> TargetIdField<T>(T value,
            string key = "target_id") => WrapField(key, value);


        private static List<Dictionary<string, object>> WrapField<T>(string key, T value)
            => new List<Dictionary<string, object>>
                  { new Dictionary<string, object> { { key, value } } };
    }
}
