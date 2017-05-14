using LiteDB;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ChangeNotification;
using Repo2.SDK.WPF45.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract class LocalRepoBase<T> : StatusChangerN45
    {
        private MemoryStream       _memStream;
        private BsonMapper         _bMapr;
        private IFileSystemAccesor _fs;
        private bool               _isSeeded;

        public LocalRepoBase(IFileSystemAccesor fileSystemAccessor)
        {
            _bMapr           = new BsonMapper();
            TypeName         = typeof(T).Name;
            CollectionName   = GetCollectionName();
            _fs              = fileSystemAccessor;
            if (_fs != null)
            {
                DatabaseFilename = GetDatabaseFilename();
                DatabaseFullPath = LocateDatabaseFile(_fs, DatabaseFilename);
                JsonSeedFilename = GetJsonSeedFilename();
                JsonSeedFullPath = LocateJsonSeedFile(_fs, DatabaseFullPath, JsonSeedFilename);
            }
            else
                _memStream = new MemoryStream();

            _bMapr.RegisterAutoId<uint>(v => v == 0,
                (db, col) => (uint)db.Count(col) + 1);
        }

        public string   TypeName           { get; }
        public string   DatabaseFilename   { get; }
        public string   DatabaseFullPath   { get; }
        public string   JsonSeedFilename   { get; }
        public string   JsonSeedFullPath   { get; }
        public string   CollectionName     { get; }
        public bool     IsSeedEnabled      { get; protected set; }

        public bool IsInMemory => _memStream != null;


        public uint Insert(T newRecord)
        {
            if (newRecord == null)
                throw Fault.NullRef<T>("record to insert");

            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                var bVal = col.Insert(newRecord);
                return (uint)bVal.AsInt64;
            }
        }


        protected void EnsureIndex<K>(Expression<Func<T, K>> indexExpression, bool isUnique)
        {
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                col.EnsureIndex<K>(indexExpression, isUnique);
            }
        }


        public List<T> Find (Expression<Func<T, bool>> predicate)
        {
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                return col.Find(predicate).ToList();
            }
        }



        public List<T> Find(Query query)
        {
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                return col.Find(query).ToList();
            }
        }


        public void Insert (IEnumerable<T> newRecords)
        {
            SetStatus($"Inserting {newRecords.Count()} ‹{TypeName}› records ...");
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                using (var trans = db.BeginTrans())
                {
                    foreach (var rec in newRecords)
                        col.Insert(rec);

                    trans.Commit();
                }
            }
            SetStatus($"Successfully inserted {newRecords.Count()} ‹{TypeName}› records.");
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
            if (filename.IsBlank()) return string.Empty;

            var path = Path.Combine(fs.CurrentExeDir, filename);
            fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }

        protected virtual string GetJsonSeedFilename()
            => "seed.json";

        protected virtual string LocateJsonSeedFile(IFileSystemAccesor fs, string dbFullPath, string jsonFilename)
        {
            if (dbFullPath.IsBlank()) return string.Empty;

            var path = Path.Combine(fs.ParentDir(dbFullPath), jsonFilename);
            fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }

        protected virtual void InsertSeedRecordsFromFile()
        {
            var jsonStr  = File.ReadAllText(JsonSeedFullPath);
            var seedRecs = Json.Deserialize<List<T>>(jsonStr);

            SetStatus($"Seeding {seedRecs.Count:N0} ‹{TypeName}› records from [{JsonSeedFilename}] ...");
            using (var db = CreateLiteDatabase())
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



        protected virtual LiteDatabase CreateLiteDatabase()
        {
            if (IsInMemory)
                return new LiteDatabase(_memStream, _bMapr);
            else
                return new LiteDatabase(ConnectString.LiteDB(DatabaseFullPath), _bMapr);
        }


        public LiteDatabase ConnectToDB(out LiteCollection<T> collection)
        {
            if (IsSeedEnabled && !_isSeeded)
            {
                if (!_fs.Found(DatabaseFullPath) 
                  && _fs.Found(JsonSeedFullPath))
                    InsertSeedRecordsFromFile();

                _isSeeded = true;
            }
            var db     = CreateLiteDatabase();
            collection = db.GetCollection<T>(CollectionName);
            return db;
        }

        //protected LiteCollection<T> _(LiteDatabase db)
        //    => db.GetCollection<T>(CollectionName);
    }
}
