using Repo2.Core.ns11.DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace Repo2.Core.ns11.Extensions.SubjectSnapshotExtensions
{
    public static class SubjectValueModExtensions
    {
        public static T Last<T>(this List<IGrouping<string, SubjectValueMod>> grpd, string fieldName)
        {
            var fGrp = grpd.SingleOrDefault(x => x.Key == fieldName);
            if (fGrp == null) return default(T);
            var objVal = fGrp.Last().NewValue;

            if (typeof(T) == typeof(ulong))
                return (T)((object)ulong.Parse(objVal.ToString()));
            else
                return (T)objVal;
        }

    }
}
