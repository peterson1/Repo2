using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Repo2.Core.ns11.Drupal8.Attributes
{
    public class _Attribute : Attribute
    {
        private static Dictionary<Type, Func<object, object>> _typFunctions = GetTypeActions();

        public _Attribute(string fieldName)
        {
            FieldName = fieldName;
        }


        public string FieldName { get; }



        internal static Dictionary<string, object> FindInPropertiesOf<T>(T sourceObj)
        {
            var dict = new Dictionary<string, object>();
            foreach (var prop in typeof(T).GetRuntimeProperties())
            {
                var d8Field = GetD8FieldAndValue(prop, sourceObj);
                if (d8Field.HasValue) dict.Add(d8Field?.Key, d8Field?.Value);
            }
            return dict;
        }


        private static KeyValuePair<string, object>? GetD8FieldAndValue(PropertyInfo prop, object sourceObj)
        {
            var attr = FindThisAttribute(prop);
            if (attr == null) return null;

            var key    = $"field_{GetFieldName(attr)}";
            var objVal = prop.GetValue(sourceObj);

            Func<object, object> func;
            if (_typFunctions.TryGetValue(prop.PropertyType, out func))
                return new KeyValuePair<string, object>
                    (key, D8HALJson.ValueField(func(objVal)));
            else
                return new KeyValuePair<string, object>
                    (key, D8HALJson.ValueField(objVal));
        }

        private static Dictionary<Type, Func<object, object>> GetTypeActions()
            => new Dictionary<Type, Func<object, object>>
            {
                { typeof(string  ), x => x?.ToString() ?? "" },
                //{ typeof(DateTime), x => ((DateTime)x).ToString("yyyy-MM-dd H:mm:ss") }
                { typeof(DateTime ), x => ToD8Date(x) },
                { typeof(DateTime?), x => ToD8Date(x) }
            };

        private static CustomAttributeData FindThisAttribute(PropertyInfo prop)
            => prop.CustomAttributes.SingleOrDefault(x
                => x.AttributeType == typeof(_Attribute));

        private static string GetFieldName(CustomAttributeData attr)
            => attr.ConstructorArguments.Single().Value.ToString();

        private static string ToD8Date(object obj)
            => obj == null ? null : ((DateTime)obj).ToString("yyyy-MM-dd H:mm:ss");
    }
}
