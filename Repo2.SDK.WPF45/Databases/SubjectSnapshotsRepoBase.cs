using LiteDB;
using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ChangeNotification;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract class SubjectSnapshotsRepoBase : StatusChangerN45, ISubjectSnapshotsDB
    {
        private IFileSystemAccesor    _fs;
        private ISubjectAlterationsDB _modsDB;
        private string                _snapsDbPath;
        //private BsonMapper            _mapr;


        protected abstract string  DbFileName      { get; }
        protected abstract string  CollectionName  { get; }


        public SubjectSnapshotsRepoBase(IFileSystemAccesor fileSystemAccessor, 
                                        ISubjectAlterationsDB subjectAlterationsDB)
        {
            _fs     = fileSystemAccessor;
            _modsDB = subjectAlterationsDB;
            //_mapr        = BsonMapper.Global;
            //_snapsDbPath = Path.Combine(fs.CurrentExeDir, DbFileName);
        }


        private LiteDatabase CreateConnection()
        {
            if (_snapsDbPath.IsBlank())
                _snapsDbPath = LocateDatabaseFile();

            return new LiteDatabase(_snapsDbPath);
        }


        private string LocateDatabaseFile()
        {
            var path = Path.Combine(_fs.CurrentExeDir, DbFileName);
            _fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }


        public async Task<T> GetLatestSnapshot<T>(int subjectId)
            where T : ISubjectSnapshot, new()
        {
            if (TryGetCached(subjectId, out T snapshot))
                return snapshot;

            snapshot = await QueryAndComposeLatestSnapshot<T>(subjectId);

            if (snapshot != null) AddToCache(snapshot);

            return snapshot;
        }


        private async Task<T> QueryAndComposeLatestSnapshot<T>(int subjectId) where T : ISubjectSnapshot, new()
        {
            var allMods = await _modsDB.GetAllMods(subjectId);
            if (allMods == null || !allMods.Any()) return default(T);
            var subj    = new T();
            subj.ApplyAlterations(allMods);
            return subj;
        }


        private void AddToCache<T>(T snapshot) where T : ISubjectSnapshot, new()
        {
            using (var db = CreateConnection())
            {
                var col = db.GetCollection<T>(CollectionName);
                col.Insert(snapshot.SubjectId, snapshot);
            }
        }


        private bool TryGetCached<T>(int subjectId, out T snapshot) 
            where T : ISubjectSnapshot, new()
        {
            using (var db = CreateConnection())
            {
                var col   = db.GetCollection<T>(CollectionName);
                col.EnsureIndex(s => s.SubjectId, true);
                var match = col.FindById(subjectId);
                snapshot  = match;
                return snapshot != null;
            }
        }


        public async Task<List<T>> GetAll<T>() where T : ISubjectSnapshot, new()
        {
            var list   = new List<T>();
            var nextId = _modsDB.GetNextSubjectId();
            if (nextId == 1) return list;

            for (int i = 1; i < nextId; i++)
            {
                var item = await this.GetLatestSnapshot<T>(i);
                if (item != null) list.Add(item);
            }
            return list;
        }
    }
}
