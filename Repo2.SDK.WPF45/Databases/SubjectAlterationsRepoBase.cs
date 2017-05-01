using LiteDB;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.Threads;
using Repo2.SDK.WPF45.ChangeNotification;
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

        protected abstract string  DbFileName      { get; }
        protected abstract string  CollectionName  { get; }


        public SubjectAlterationsRepoBase(IFileSystemAccesor fileSystemAccesor)
        {
            _fs = fileSystemAccesor;
        }


        private LiteDatabase CreateConnection()
        {
            if (_dbPath.IsBlank())
                _dbPath = LocateDatabaseFile();

            return new LiteDatabase(ConnectString.LiteDB(_dbPath));
        }


        private string LocateDatabaseFile()
        {
            var path = Path.Combine(_fs.CurrentExeDir, DbFileName);
            _fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }


        public async Task<int> CreateNewSubject(SubjectAlterations mods)
        {
            var newId = InsertSeedRow(mods);

            await NewThread.WaitFor(() =>
            {
                using (var db = CreateConnection())
                {
                    var col = db.GetCollection<SubjectValueMod>(CollectionName);

                    using (var trans = db.BeginTrans())
                    {
                        for (int i = 1; i < mods.Count; i++)
                        {
                            var row = mods[i];
                            row.SubjectID = newId;
                            col.Insert(row);
                        }
                        trans.Commit();
                    }
                }
            });

            return newId;
        }



        public async Task<IEnumerable<SubjectValueMod>> GetAllMods(int subjectId)
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



        /// <summary>
        /// Stores just one (1) row from the list.
        /// Use this to reserve the next subject id.
        /// </summary>
        /// <param name="mods"></param>
        /// <param name="rowIndexForSeed"></param>
        /// <returns></returns>
        private int InsertSeedRow(SubjectAlterations mods, int rowIndexForSeed = 0)
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


        public int GetNextSubjectId()
        {
            using (var db = CreateConnection())
            {
                var col = db.GetCollection<SubjectValueMod>(CollectionName);
                if (col.Count() == 0) return 1;

                col.EnsureIndex(e => e.SubjectID, false);
                return col.Max(e => e.SubjectID) + 1;
            }
        }
    }
}
