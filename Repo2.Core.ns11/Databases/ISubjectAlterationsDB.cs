using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.Databases
{
    public interface ISubjectAlterationsDB : IStatusChanger
    {
        Task<int>                           CreateNewSubject (SubjectAlterations mods);
        Task<IEnumerable<SubjectValueMod>>  GetAllMods       (int subjectId);
        int                                 GetNextSubjectId ();
    }
}
