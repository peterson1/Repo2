using System;
using System.Collections.Generic;

namespace Repo2.Core.ns11.Extensions
{
    //http://stackoverflow.com/a/2601501/3973863
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>
            (this IDictionary<TKey, TValue> dictionary,
             TKey key,
             TValue defaultValue = default(TValue))
        {
            TValue value;
            if (key == null) return defaultValue;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static TValue GetOrDefault<TKey, TValue>
            (this IDictionary<TKey, TValue> dictionary,
             TKey key,
             Func<TValue> defaultValueProvider)
        {
            TValue value;
            if (key == null) return defaultValueProvider();
            return dictionary.TryGetValue(key, out value) ? value
                 : defaultValueProvider();
        }
    }
}
