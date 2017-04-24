using Repo2.Core.ns11.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.Databases
{
    public interface ISubjectSnapshotsDB
    {
        Task<T> GetLatestSnapshot<T>(int subjectId)
            where T : ISubjectSnapshot, new();
    }
}
