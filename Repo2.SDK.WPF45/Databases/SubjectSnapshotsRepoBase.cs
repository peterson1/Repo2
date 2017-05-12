using LiteDB;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Exceptions;
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

        protected virtual  string  GetActorName    (int actorID) => string.Empty;


        public SubjectSnapshotsRepoBase(IFileSystemAccesor fileSystemAccessor, 
                                        ISubjectAlterationsDB subjectAlterationsDB)
        {
            _fs     = fileSystemAccessor;
            _bMapr  = new BsonMapper();
            _modsDB = subjectAlterationsDB;
            _modsDB.SubjectCreated += (s, e) => CreateSnapshot(e);
            _modsDB.SubjectUpdated += (s, e) => UpdateSnapshot(e);
        }


        private string LocateDatabaseFile()
        {
            var path = Path.Combine(_fs.CurrentExeDir, DbFileName);
            _fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }


        private T ComposeSubject(IEnumerable<SubjectValueMod> mods, uint subjectId)
        {
            var subj          = new T();
            //subj.Id           = mods.First().SubjectID;
            subj.Id           = subjectId;
            subj.DateModified = mods.Last().Timestamp;
            subj.ModifiedBy   = GetActorName(mods.Last().ActorID);

            subj.ApplyAlterations(mods);

            return subj;
        }


        private void CreateSnapshot(Tuple<SubjectAlterations, uint> tuple)
        {
            //var mods          = tuple.Item1;
            //var subj          = new T();
            //subj.Id           = mods.First().SubjectID;
            //subj.DateModified = mods.Last().Timestamp;
            //subj.ModifiedBy   = GetActorName(mods.Last().ActorID);
            //subj.ApplyAlterations(mods);
            var subj = ComposeSubject(tuple.Item1, tuple.Item2);

            using (var db = CreateConnection(out LiteCollection<T> col))
            {
                //col.Insert((long)tuple.Item2, subj);
                col.Insert((long)subj.Id, subj);
            }
        }


        private void UpdateSnapshot(List<SubjectValueMod> mods)
        {
            var subjId = mods.Last().SubjectID;
            var subj   = ComposeSubject(mods, subjId);
            using (var db = CreateConnection(out LiteCollection<T> col))
            {
                if (!col.Update((long)subjId, subj))
                    throw Fault.Failed($"Update failed. Id not found: [{subjId}].");
            }
        }


        public List<T> GetAll()
        {
            using (var db = CreateConnection(out LiteCollection<T> col))
                return col.FindAll().ToList();
        }


        private LiteDatabase CreateConnection(out LiteCollection<T> col)
        {
            if (_snapsDbPath.IsBlank())
                _snapsDbPath = LocateDatabaseFile();

            var db = new LiteDatabase(ConnectString.LiteDB(_snapsDbPath), _bMapr);
            col    = db.GetCollection<T>(CollectionName);
            return db;
        }
    }
}
