using System;
using System.Collections.Generic;

namespace Repo2.Core.ns11.EnumTools
{
    public class EnumTool
    {
        public static List<T> List<T>()
        {
            var list = new List<T>();

            foreach (T item in Enum.GetValues(typeof(T)))
                list.Add(item);

            return list;
        }
    }
}
