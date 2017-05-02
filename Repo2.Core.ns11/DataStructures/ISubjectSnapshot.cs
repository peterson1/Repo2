using System.Collections.Generic;

namespace Repo2.Core.ns11.DataStructures
{
    public interface ISubjectSnapshot
    {
        uint SubjectId { get; }

        void ApplyAlterations(IEnumerable<SubjectValueMod> mods);
    }
}
