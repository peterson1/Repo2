using LiteDB;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ChangeNotification;
using Repo2.SDK.WPF45.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract class LocalRepoBase<T> : StatusChangerN45
    {
        private BsonMapper         _bMapr;
        private IFileSystemAccesor _fs;
        private bool               _isSeeded;

        public LocalRepoBase(IFileSystemAccesor fileSystemAccessor)
        {
            _fs              = fileSystemAccessor;
            _bMapr           = new BsonMapper();
            TypeName         = typeof(T).Name;
            CollectionName   = GetCollectionName();

            DatabaseFilename = GetDatabaseFilename();
            DatabaseFullPath = LocateDatabaseFile(_fs, DatabaseFilename);
            JsonSeedFilename = GetJsonSeedFilename();
            JsonSeedFullPath = LocateJsonSeedFile(_fs, DatabaseFullPath, JsonSeedFilename);

            _bMapr.RegisterAutoId<uint>(v => v == 0,
                (db, col) => (uint)db.Count(col) + 1);
        }

        public string   TypeName           { get; }
        public string   DatabaseFilename   { get; }
        public string   DatabaseFullPath   { get; }
        public string   JsonSeedFilename   { get; }
        public string   JsonSeedFullPath   { get; }
        public string   CollectionName     { get; }


        public uint Insert(T newRecord)
        {
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                var bVal = col.Insert(newRecord);
                return (uint)bVal.AsInt64;
            }
        }


        public List<T> FindAll()
        {
            List<T> list;
            SetStatus($"Querying ALL ‹{TypeName}› records ...");

            using (var db = ConnectToDB(out LiteCollection<T> col))
                list = col.FindAll().ToList();

            SetStatus($"Query returned {list.Count:N0} ‹{TypeName}› records.");
            return list;
        }


        public T FindById(uint id)
        {
            using (var db = ConnectToDB(out LiteCollection<T> col))
                return col.FindById((long)id);
        }


        protected virtual string GetDatabaseFilename() 
            => $"LocalRepo_{TypeName}.LiteDB3";

        protected virtual string LocateDatabaseFile(IFileSystemAccesor fs, string filename)
        {
            var path = Path.Combine(fs.CurrentExeDir, filename);
            fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }

        protected virtual string GetJsonSeedFilename()
            => "seed.json";

        protected virtual string LocateJsonSeedFile(IFileSystemAccesor fs, string dbFullPath, string jsonFilename)
        {
            var path = Path.Combine(fs.ParentDir(dbFullPath), jsonFilename);
            fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }

        protected virtual void InsertSeedRecordsFromFile()
        {
            var jsonStr  = File.ReadAllText(JsonSeedFullPath);
            var seedRecs = Json.Deserialize<List<T>>(jsonStr);

            SetStatus($"Seeding {seedRecs.Count:N0} ‹{TypeName}› records from [{JsonSeedFilename}] ...");
            using (var db = new LiteDatabase(ConnectString.LiteDB(DatabaseFullPath), _bMapr))
            {
                var col = db.GetCollection<T>(CollectionName);
                using (var trans = db.BeginTrans())
                {
                    foreach (var rec in seedRecs)
                        if (rec != null) col.Insert(rec);

                    trans.Commit();
                }
            }
            SetStatus($"Successfully inserted ‹{TypeName}› records from seed file.");
        }

        protected virtual string GetCollectionName()
            => "v1";



        protected LiteDatabase ConnectToDB(out LiteCollection<T> collection)
        {
            if (!_isSeeded)
            {
                if (!_fs.Found(DatabaseFullPath) 
                  && _fs.Found(JsonSeedFullPath))
                    InsertSeedRecordsFromFile();

                _isSeeded = true;
            }
            var db     = new LiteDatabase(ConnectString.LiteDB(DatabaseFullPath), _bMapr);
            collection = db.GetCollection<T>(CollectionName);
            return db;
        }

        //protected LiteCollection<T> _(LiteDatabase db)
        //    => db.GetCollection<T>(CollectionName);
    }
}
