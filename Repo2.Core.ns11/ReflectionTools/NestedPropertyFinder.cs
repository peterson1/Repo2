using System;
using System.Reflection;

namespace Repo2.Core.ns11.ReflectionTools
{
    public static class NestedPropertyFinder
    {
        public static PropertyInfo FindProperty(this Type type, string propertyName)
        {
            if (type.GetTypeInfo().IsInterface)
                return FindInterfaceProperty(type, propertyName);
            else
                return type.GetRuntimeProperty(propertyName);
        }


        private static PropertyInfo FindInterfaceProperty(Type type, string propertyName)
        {
            var prop = type.GetRuntimeProperty(propertyName);
            if (prop != null) return prop;

            foreach (var subTyp in type.GetTypeInfo().ImplementedInterfaces)
            {
                var subProp = FindInterfaceProperty(subTyp, propertyName);
                if (subProp != null) return subProp;
            }

            return null;
        }
    }
}
