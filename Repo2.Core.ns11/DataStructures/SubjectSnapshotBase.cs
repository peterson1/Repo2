using System.Collections.Generic;
using System.Linq;

namespace Repo2.Core.ns11.DataStructures
{
    public abstract class SubjectSnapshotBase : ISubjectSnapshot
    {
        public abstract uint SubjectId { get; }

        public abstract void ApplyAlterations(IEnumerable<SubjectValueMod> mods);

        public abstract SubjectAlterations ListAlterations(int actorId);

        protected T Last<T>(string fieldName, List<IGrouping<string, SubjectValueMod>> grpd)
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
