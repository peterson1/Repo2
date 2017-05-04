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
        private IFileSystemAccesor    _fs;
        private ISubjectAlterationsDB _modsDB;
        private string                _snapsDbPath;
        private BsonMapper            _bMapr;


        protected abstract string  DbFileName      { get; }
        protected abstract string  CollectionName  { get; }


        public SubjectSnapshotsRepoBase(IFileSystemAccesor fileSystemAccessor, 
                                        ISubjectAlterationsDB subjectAlterationsDB)
        {
            _fs     = fileSystemAccessor;
            _modsDB = subjectAlterationsDB;
            _bMapr  = new BsonMapper();
        }


        private LiteDatabase CreateConnection()
        {
            if (_snapsDbPath.IsBlank())
                _snapsDbPath = LocateDatabaseFile();

            return new LiteDatabase(ConnectString.LiteDB(_snapsDbPath), _bMapr);
        }


        private string LocateDatabaseFile()
        {
            var path = Path.Combine(_fs.CurrentExeDir, DbFileName);
            _fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }


        public async Task<T> GetLatestSnapshot<T>(uint subjectId)
            where T : ISubjectSnapshot, new()
        {
            if (TryGetCached(subjectId, out T snapshot))
                return snapshot;

            snapshot = await QueryAndComposeLatestSnapshotAsync<T>(subjectId);

            if (snapshot != null) AddToCache(snapshot);

            return snapshot;
        }


        private async Task<T> QueryAndComposeLatestSnapshotAsync<T>(uint subjectId) where T : ISubjectSnapshot, new()
        {
            var allMods = await _modsDB.GetAllModsAsync(subjectId);
            if (allMods == null || !allMods.Any()) return default(T);
            var subj    = new T();
            subj.ApplyAlterations(allMods);
            return subj;
        }


        private T QueryAndComposeLatestSnapshot<T>(uint subjectId) where T : ISubjectSnapshot, new()
        {
            var allMods = _modsDB.GetAllMods(subjectId);
            if (allMods == null || !allMods.Any()) return default(T);
            var subj = new T();
            subj.ApplyAlterations(allMods);
            return subj;
        }


        private void AddToCache<T>(T snapshot) where T : ISubjectSnapshot, new()
        {
            using (var db = CreateConnection())
            {
                var col = db.GetCollection<T>(CollectionName);
                col.Insert((long)snapshot.Id, snapshot);
            }
        }


        private bool TryGetCached<T>(uint subjectId, out T snapshot) 
            where T : ISubjectSnapshot, new()
        {
            using (var db = CreateConnection())
            {
                var col   = db.GetCollection<T>(CollectionName);
                //col.EnsureIndex(s => s.Id, true);
                var match = col.FindById((long)subjectId);
                snapshot  = match;
                return snapshot != null;
            }
        }


        //public async Task<List<T>> GetAll<T>() where T : ISubjectSnapshot, new()
        //{
        //    var list   = new List<T>();
        //    var nextId = _modsDB.GetNextSubjectId();
        //    if (nextId == 1) return list;

        //    for (uint i = 1; i < nextId; i++)
        //    {
        //        var item = await this.GetLatestSnapshot<T>(i);
        //        if (item != null) list.Add(item);
        //    }
        //    return list;
        //}
    }
}
