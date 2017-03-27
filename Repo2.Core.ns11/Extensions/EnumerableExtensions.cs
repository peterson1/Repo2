using System;
using System.Linq;
using System.Collections.Generic;
using Repo2.Core.ns11.Exceptions;

namespace Repo2.Core.ns11.Extensions
{
    public static class EnumerableExtensions
    {
        //public static Dictionary<TKey, TVal> ToDictionary<TKey, TVal>(this IEnumerable<TVal> colxn, Func<TVal, TKey> keySelector)
        //{
        //    var dict = new Dictionary<TKey, TVal>();

        //    foreach (var item in colxn)
        //        dict.Add(keySelector(item), item);

        //    return dict;
        //}

        public static T SingleOrDefault<T>(this IEnumerable<T> items,
            Func<T, bool> predicate, string key, object value, string operatr = "==")
        {
            var matches = items.Where(predicate);

            if (matches.Count() == 0) return default(T);
            if (matches.Count() == 1) return matches.First();

            throw Fault.MultiMatch<T>(key, value, operatr);
        }
    }
}
