using System;

namespace Repo2.Core.ns11.ReflectionTools
{
    public static class TypeExtensions
    {

        // http://stackoverflow.com/a/1398934
        public static bool IsNullableType(this Type type)
            => type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
    }
}
