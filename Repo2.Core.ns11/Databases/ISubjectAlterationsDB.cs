using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.Databases
{
    public interface ISubjectAlterationsDB : IStatusChanger
    {
        Task<uint>                          CreateNewSubject (SubjectAlterations mods);
        Task<IEnumerable<SubjectValueMod>>  GetAllModsAsync  (uint subjectId);
        List<SubjectValueMod>               GetAllMods       (uint subjectId);
        uint                                GetNextSubjectId ();
    }
}
