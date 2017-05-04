using LiteDB;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ChangeNotification;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract partial class SubjectSnapshotsRepoBase : StatusChangerN45, ISubjectSnapshotsDB
    {
        public List<T> GetAll<T>() where T : ISubjectSnapshot, new()
        {
            var list   = new List<T>();
            var nextId = _modsDB.GetNextSubjectId();
            if (nextId == 1) return list;

            using (var db = CreateConnection())
            {
                var col = db.GetCollection<T>(CollectionName);
                //col.EnsureIndex(s => s.GetSubjectId(), true);

                using (var trans = db.BeginTrans())
                {
                    for (uint i = 1; i < nextId; i++)
                    {
                        var snapshot = col.FindById((long)i);
                        if (snapshot == null)
                        {
                            snapshot = QueryAndComposeLatestSnapshot<T>(i);
                            if (snapshot != null) col.Insert((long)i, snapshot);
                        }
                        if (snapshot != null) list.Add(snapshot);
                    }
                    trans.Commit();
                }
            }
            return list;
        }
    }
}
