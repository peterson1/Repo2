using LiteDB;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ChangeNotification;
using System.IO;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract partial class LocalRepoBase<T> : StatusChangerN45
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



        protected virtual string GetDatabaseFilename() => $"LocalRepo_{TypeName}.LiteDB3";
        protected virtual string GetCollectionName  () => "v1";
        protected virtual string GetJsonSeedFilename() => "seed.json";


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


        protected virtual string LocateDatabaseFile(IFileSystemAccesor fs, string filename)
        {
            if (filename.IsBlank()) return string.Empty;

            var path = Path.Combine(fs.CurrentExeDir, filename);
            fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }


        protected virtual string LocateJsonSeedFile(IFileSystemAccesor fs, string dbFullPath, string jsonFilename)
        {
            if (dbFullPath.IsBlank()) return string.Empty;

            var path = Path.Combine(fs.ParentDir(dbFullPath), jsonFilename);
            fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }


        protected virtual LiteDatabase CreateLiteDatabase()
        {
            if (IsInMemory)
                return new LiteDatabase(_memStream, _bMapr);
            else
                return new LiteDatabase(ConnectString.LiteDB(DatabaseFullPath), _bMapr);
        }


    }
}
