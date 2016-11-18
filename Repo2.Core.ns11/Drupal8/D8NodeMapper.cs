using System.Collections.Generic;
using Repo2.Core.ns11.Drupal8.Attributes;

namespace Repo2.Core.ns11.Drupal8
{
    public class D8NodeMapper
    {
        public static Dictionary<string, object> Cast<T>(T sourceObj, string baseUrl) where T : D8NodeBase
        {
            var dict = new Dictionary<string, object>();

            dict.Add("title", ContentTitleAttribute.ToD8Field(sourceObj));
            dict.Add("type", D8HALJson.TargetIdField(sourceObj.D8TypeName));
            dict.Add("_links", D8HALJson.GetNodeLinks(sourceObj, baseUrl));

            foreach (var field in _Attribute.FindInPropertiesOf(sourceObj))
                dict.Add(field.Key, field.Value);

            return dict;
        }
    }
}
