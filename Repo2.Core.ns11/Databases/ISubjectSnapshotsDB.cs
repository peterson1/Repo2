using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using System.Threading.Tasks;

namespace Repo2.Core.ns11.Databases
{
    public interface ISubjectSnapshotsDB : IStatusChanger
    {
        Task<T> GetLatestSnapshot<T>(int subjectId)
            where T : ISubjectSnapshot, new();
    }
}
