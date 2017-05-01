using System.Collections.Generic;
using System.Linq;

namespace Repo2.Core.ns11.DataStructures
{
    public abstract class SubjectSnapshotBase : ISubjectSnapshot
    {
        public abstract int SubjectId { get; }

        public abstract void ApplyAlterations(IEnumerable<SubjectValueMod> mods);


        protected T Last<T>(string fieldName, List<IGrouping<string, SubjectValueMod>> grpd)
        {
            var fGrp = grpd.SingleOrDefault(x => x.Key == fieldName);
            if (fGrp == null) return default(T);
            return (T)fGrp.Last().NewValue;
        }
    }
}
