using System;
using System.Linq;
using System.Reflection;

namespace Repo2.Core.ns11.ReflectionTools
{
    public static class AssemblyReflector
    {
        //public static List<string> GetNamespaces(this Assembly assembly)
        //    => assembly.ExportedTypes.Select(x 
        //        => x.Namespace).Distinct().ToList();


        public static string PrependNamespace(this Assembly assembly, string typeName)
        {
            var typ = assembly.ExportedTypes.FirstOrDefault(x => x.Name == typeName);
            if (typ == null) return null;

            return $"{typ.Namespace}.{typ.Name}";
        }


        public static Type FindTypeByName(this Assembly assembly, string typeName, bool errorIfMissing)
        {
            var fullNme = assembly.PrependNamespace(typeName);
            if (fullNme == null)
            {
                if (!errorIfMissing) return null;
                throw new TypeAccessException($"Failed to find type: “{typeName}”.");
            }
            try
            {
                return assembly.GetType(fullNme);
            }
            catch (Exception ex)
            {
                if (errorIfMissing) throw ex;
                return null;
            }
        }
    }
}
