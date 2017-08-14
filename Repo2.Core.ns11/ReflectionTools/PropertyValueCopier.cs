using System;
using System.Linq;
using System.Reflection;

namespace Repo2.Core.ns11.ReflectionTools
{
    public static class PropertyValueCopier
    {
        public static void CopyByNameFrom<TDestination, TSource>(this TDestination targetObj, TSource sourceObj)
        {
            if (targetObj == null || sourceObj == null) return;

            var writableProps = typeof(TDestination)
                .GetRuntimeProperties().Where(x => x.CanWrite).ToList();

            foreach (var destProp in writableProps)
            {
                //var srcProp = typeof(TSource).GetRuntimeProperty(destProp.Name);
                var srcProp = typeof(TSource).FindProperty(destProp.Name);
                if (srcProp == null) continue;
                var srcVal = srcProp.GetValue(sourceObj);

                try
                {
                    destProp.SetValue(targetObj, srcVal);
                }
                catch (Exception) { }
            }
        }
    }
}
