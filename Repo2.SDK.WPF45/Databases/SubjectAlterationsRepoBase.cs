﻿using LiteDB;
using Repo2.Core.ns11.ChangeNotification;
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
    public abstract class SubjectAlterationsRepoBase : StatusChangerN45, ISubjectAlterationsDB
    {
        private      EventHandler<Tuple<SubjectAlterations, uint>> _subjectCreated;
        public event EventHandler<Tuple<SubjectAlterations, uint>>  SubjectCreated
        {
            add
            {
                value.TryRemoveFrom(ref _subjectCreated);
                _subjectCreated += value;
            }
            remove { _subjectCreated -= value; }
        }

        private      EventHandler<List<SubjectValueMod>> _subjectUpdated;
        public event EventHandler<List<SubjectValueMod>>  SubjectUpdated
        {
            add
            {
                value.TryRemoveFrom(ref _subjectUpdated);
                _subjectUpdated += value;
            }
            remove { _subjectUpdated -= value; }
        }

        private string             _dbPath;
        private IFileSystemAccesor _fs;
        private BsonMapper         _bMapr;

        protected abstract string  DbFileName      { get; }
        protected abstract string  CollectionName  { get; }


        //public Func<SubjectAlterations, uint, Reply> PostCreateValidator { get; set; }


        public SubjectAlterationsRepoBase(IFileSystemAccesor fileSystemAccesor)
        {
            _fs    = fileSystemAccesor;
            _bMapr = new BsonMapper();

            _bMapr.RegisterAutoId<ulong>(v => v == 0, 
                (db, col) => (ulong)db.Count(col) + 1);
        }


        public uint CreateNewSubject(SubjectAlterations mods)
        {
            var withValues = mods.Where(x => HasValue(x)).ToList();
            if (!withValues.Any())
                throw Fault.BadData(mods, "No non-NULL values in list.");

            var newId = InsertSeedRow(withValues, 0);

            using (var db = ConnectToDB(out LiteCollection<SubjectValueMod> col))
            {
                using (var trans = db.BeginTrans())
                {
                    for (int i = 1; i < withValues.Count; i++)
                    {
                        var row = withValues[i];
                        row.SubjectID = newId;
                        col.Insert(row);
                    }
                    trans.Commit();
                }
            }
            _subjectCreated.Raise(Tuple.Create(mods, newId));
            return newId;
        }


        public bool UpdateSubject(SubjectAlterations newValues)
        {
            if (newValues == null) return false;
            if (!newValues.Any()) return false;

            using (var db = ConnectToDB(out LiteCollection<SubjectValueMod> col))
            {
                var oldValues = FindMods(col, newValues.SubjectID);
                var changes   = newValues.GetChanges(oldValues);
                if (!changes.Any()) return false;

                col.Insert(changes);

                var mergd = oldValues.Concat(changes).ToList();
                _subjectUpdated.Raise(mergd);
            }
            return true;
        }



        private List<SubjectValueMod> FindMods(LiteCollection<SubjectValueMod> col, uint subjectId)
        {
            col.EnsureIndex(x => x.SubjectID);
            return col.Find    (x => x.SubjectID == subjectId)
                      .OrderBy (x => x.Timestamp)
                      .ToList  ();
        }


        private bool HasValue(SubjectValueMod mod)
        {
            if (mod.NewValue == null) return false;
            if (mod.NewValue.ToString().Trim().IsBlank()) return false;
            return true;
        }



        /// <summary>
        /// Stores just one (1) row from the list.
        /// Use this to reserve the next subject id.
        /// </summary>
        /// <param name="mods"></param>
        /// <param name="rowIndexForSeed"></param>
        /// <returns></returns>
        private uint InsertSeedRow(List<SubjectValueMod> mods, int rowIndexForSeed)
        {
            var seed = mods[rowIndexForSeed];

            using (var db = ConnectToDB(out LiteCollection<SubjectValueMod> col))
            {
                if (col.Count() == 0)
                    seed.SubjectID = 1;
                else
                {
                    col.EnsureIndex(e => e.SubjectID, false);
                    var bsonVal = col.Max(e => e.SubjectID);
                    seed.SubjectID = (uint)(bsonVal.AsInt64 + 1);
                }
                col.Insert(seed);
            }
            return seed.SubjectID;
        }




        private LiteDatabase ConnectToDB(out LiteCollection<SubjectValueMod> col)
        {
            if (_dbPath.IsBlank())
                _dbPath = LocateDatabaseFile();

            var db = new LiteDatabase(ConnectString.LiteDB(_dbPath), _bMapr);
            col    = db.GetCollection<SubjectValueMod>(CollectionName);
            return db;
        }


        private string LocateDatabaseFile()
        {
            var path = Path.Combine(_fs.CurrentExeDir, DbFileName);
            _fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }

    }
}
