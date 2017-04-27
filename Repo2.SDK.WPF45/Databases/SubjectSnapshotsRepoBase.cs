using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.FileSystems;
using System.IO;
using System.Threading.Tasks;
using System;
using LiteDB;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract class SubjectSnapshotsRepoBase : ISubjectSnapshotsDB
    {
        private string                _snapsDbPath;
        private ISubjectAlterationsDB _modsDB;
        private BsonMapper            _mapr;


        protected abstract string  DbFileName      { get; }
        protected abstract string  CollectionName  { get; }


        public SubjectSnapshotsRepoBase(IFileSystemAccesor fs, 
                                        ISubjectAlterationsDB subjectAlterationsDB)
        {
            _mapr        = BsonMapper.Global;
            _modsDB      = subjectAlterationsDB;
            _snapsDbPath = Path.Combine(fs.CurrentExeDir, DbFileName);
        }


        public async Task<T> GetLatestSnapshot<T>(int subjectId)
            where T : ISubjectSnapshot, new()
        {
            if (TryGetCached(subjectId, out T snapshot))
                return snapshot;

            snapshot = await QueryAndComposeLatestSnapshot<T>(subjectId);

            AddToCache(snapshot);

            return snapshot;
        }


        private async Task<T> QueryAndComposeLatestSnapshot<T>(int subjectId) where T : ISubjectSnapshot, new()
        {
            var allMods = await _modsDB.GetAllMods(subjectId);
            var subj    = new T();
            subj.ApplyAlterations(allMods);
            return subj;
        }


        private void AddToCache<T>(T snapshot) where T : ISubjectSnapshot, new()
        {
            using (var db = new LiteDatabase(_snapsDbPath, _mapr))
            {
                var col = db.GetCollection<T>(CollectionName);
                col.Insert(snapshot.SubjectId, snapshot);
            }
        }


        private bool TryGetCached<T>(int subjectId, out T snapshot) 
            where T : ISubjectSnapshot, new()
        {
            using (var db = new LiteDatabase(_snapsDbPath, _mapr))
            {
                var col   = db.GetCollection<T>(CollectionName);
                col.EnsureIndex(s => s.SubjectId, true);
                var match = col.FindById(subjectId);
                snapshot  = match;
                return snapshot != null;
            }
        }
    }
}
