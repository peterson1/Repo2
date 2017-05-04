﻿using System.Collections.Generic;

namespace Repo2.Core.ns11.DataStructures
{
    public interface ISubjectSnapshot
    {
        uint Id { get; }

        void                ApplyAlterations  (IEnumerable<SubjectValueMod> mods);
        SubjectAlterations  ListAlterations   (int actorId);
    }
}
