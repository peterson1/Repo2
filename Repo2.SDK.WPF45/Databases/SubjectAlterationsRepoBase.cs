using LiteDB;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.Threads;
using Repo2.SDK.WPF45.ChangeNotification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract class SubjectAlterationsRepoBase : StatusChangerN45, ISubjectAlterationsDB
    {
        private string             _dbPath;
        private IFileSystemAccesor _fs;
        private BsonMapper         _bMapr;

        protected abstract string  DbFileName      { get; }
        protected abstract string  CollectionName  { get; }


        public SubjectAlterationsRepoBase(IFileSystemAccesor fileSystemAccesor)
        {
            _fs    = fileSystemAccesor;
            _bMapr = new BsonMapper();

            _bMapr.RegisterAutoId<ulong>(v => v == 0, 
                (db, col) => (ulong)db.Count(col) + 1);
        }


        private LiteDatabase CreateConnection()
        {
            if (_dbPath.IsBlank())
                _dbPath = LocateDatabaseFile();

            return new LiteDatabase(ConnectString.LiteDB(_dbPath), _bMapr);
        }


        private string LocateDatabaseFile()
        {
            var path = Path.Combine(_fs.CurrentExeDir, DbFileName);
            _fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }


        public async Task<uint> CreateNewSubject(SubjectAlterations mods)
        {
            var withValues = mods.Where(x => HasValue(x)).ToList();
            if (!withValues.Any())
                throw Fault.BadData(mods, "No non-NULL values in list.");

            var newId = InsertSeedRow(withValues);

            await NewThread.WaitFor(() =>
            {
                using (var db = CreateConnection())
                {
                    var col = db.GetCollection<SubjectValueMod>(CollectionName);

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
            });

            return newId;
        }


        private bool HasValue(SubjectValueMod mod)
        {
            if (mod.NewValue == null) return false;
            if (mod.NewValue.ToString().Trim().IsBlank()) return false;
            return true;
        }


        public async Task<IEnumerable<SubjectValueMod>> GetAllModsAsync(uint subjectId)
        {
            using (var db = CreateConnection())
            {
                var col = db.GetCollection<SubjectValueMod>(CollectionName);

                await NewThread.WaitFor(()
                    => col.EnsureIndex(m => m.SubjectID));

                var mods = await NewThread.WaitFor(()
                    => col.Find(_ => _.SubjectID == subjectId));

                if (mods == null || !mods.Any())
                    return new List<SubjectValueMod>();

                return mods.OrderBy(x => x.Timestamp).ToList();
            }
        }


        public List<SubjectValueMod> GetAllMods(uint subjectId)
        {
            using (var db = CreateConnection())
            {
                var col = db.GetCollection<SubjectValueMod>(CollectionName);

                col.EnsureIndex(m => m.SubjectID);

                var mods = col.Find(_ => _.SubjectID == subjectId);

                if (mods == null || !mods.Any())
                    return new List<SubjectValueMod>();

                return mods.OrderBy(x => x.Timestamp).ToList();
            }
        }



        /// <summary>
        /// Stores just one (1) row from the list.
        /// Use this to reserve the next subject id.
        /// </summary>
        /// <param name="mods"></param>
        /// <param name="rowIndexForSeed"></param>
        /// <returns></returns>
        private uint InsertSeedRow(List<SubjectValueMod> mods, int rowIndexForSeed = 0)
        {
            var newId      = GetNextSubjectId();
            var seed       = mods[rowIndexForSeed];
            seed.SubjectID = newId;
            InsertSolo(seed);
            return newId;
        }


        private void InsertSolo(SubjectValueMod eventRow)
        {
            using (var db = CreateConnection())
            {
                var col = db.GetCollection<SubjectValueMod>(CollectionName);
                col.Insert(eventRow);
            }
        }


        public uint GetNextSubjectId()
        {
            using (var db = CreateConnection())
            {
                var col = db.GetCollection<SubjectValueMod>(CollectionName);
                if (col.Count() == 0) return 1;

                col.EnsureIndex(e => e.SubjectID, false);
                var bsonVal = col.Max(e => e.SubjectID);
                return (uint)(bsonVal.AsInt64 + 1);
            }
        }
    }
}
