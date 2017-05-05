using LiteDB;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ChangeNotification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract class SubjectSnapshotsRepoBase<T> : StatusChangerN45, ISubjectSnapshotsDB<T>
        where T : ISubjectSnapshot, new()
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
            _bMapr  = new BsonMapper();
            _modsDB = subjectAlterationsDB;
            _modsDB.SubjectCreated += (s, e) => CreateSnapshot(e);
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


        private void CreateSnapshot(Tuple<SubjectAlterations, uint> tuple)
        {
            var subj = new T();
            subj.ApplyAlterations(tuple.Item1);
            using (var db = CreateConnection())
            {
                var col = db.GetCollection<T>(CollectionName);
                col.Insert((long)tuple.Item2, subj);
            }
        }



        public List<T> GetAll()
        {
            using (var db = CreateConnection())
            {
                var col = db.GetCollection<T>(CollectionName);
                return col.FindAll().ToList();
            }
        }


        //private T QueryAndComposeLatestSnapshot(uint subjectId)
        //{
        //    var allMods = _modsDB.GetAllMods(subjectId);
        //    if (allMods == null || !allMods.Any()) return default(T);
        //    var subj = new T();
        //    subj.ApplyAlterations(allMods);
        //    return subj;
        //}


        //public List<T> GetAll()
        //{
        //    var list   = new List<T>();
        //    var nextId = _modsDB.GetNextSubjectId();
        //    if (nextId == 1) return list;

        //    using (var db = CreateConnection())
        //    {
        //        var col = db.GetCollection<T>(CollectionName);

        //        using (var trans = db.BeginTrans())
        //        {
        //            for (uint i = 1; i < nextId; i++)
        //            {
        //                var snapshot = col.FindById((long)i);
        //                if (snapshot == null)
        //                {
        //                    snapshot = QueryAndComposeLatestSnapshot(i);
        //                    if (snapshot != null) col.Insert((long)i, snapshot);
        //                }
        //                if (snapshot != null) list.Add(snapshot);
        //            }
        //            trans.Commit();
        //        }
        //    }
        //    return list;
        //}
    }
}
