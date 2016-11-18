using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Repo2.Core.ns11.Drupal8.Attributes
{
    public class _Attribute : Attribute
    {
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

            var key = $"field_{GetFieldName(attr)}";
            var val = prop.GetValue(sourceObj);

            return new KeyValuePair<string, object>
                (key, D8HALJson.ValueField(val));
        }

        private static CustomAttributeData FindThisAttribute(PropertyInfo prop)
            => prop.CustomAttributes.SingleOrDefault(x
                => x.AttributeType == typeof(_Attribute));

        private static string GetFieldName(CustomAttributeData attr)
            => attr.ConstructorArguments.Single().Value.ToString();
    }
}
