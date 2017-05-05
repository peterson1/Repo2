using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.Databases
{
    public interface ISubjectSnapshotsDB<T> : IStatusChanger
        where T : ISubjectSnapshot, new()
    {
        //T GetLatestSnapshot<T>(uint subjectId)
        //    where T : ISubjectSnapshot, new();

        List<T> GetAll();
    }
}
