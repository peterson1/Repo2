﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.DataStructures
{
    public interface ISubjectSnapshot
    {
        int SubjectId { get; }

        void ApplyAlterations(IEnumerable<SubjectValueMod> allMods);
    }
}