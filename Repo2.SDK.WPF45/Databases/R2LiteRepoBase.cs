using LiteDB;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ChangeNotification;
using System.IO;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract class R2LiteRepoBase : StatusChangerN45
    {
        private   MemoryStream       _memStream;
        private   BsonMapper         _bMapr;
        protected IFileSystemAccesor _fs;


        public R2LiteRepoBase(IFileSystemAccesor fileSystemAccessor)
        {
            _fs = fileSystemAccessor;
            _bMapr = new BsonMapper();

            if (_fs == null)
                _memStream = new MemoryStream();

            _bMapr.RegisterAutoId<uint>(v => v == 0,
                (db, col) => (uint)db.Count(col) + 1);
        }

        public string   DatabaseFilename   { get; private set; }
        public string   DatabaseFullPath   { get; private set; }

        public bool IsInMemory => _memStream != null;


        protected abstract string GetDatabaseFilename();


        protected virtual string LocateDatabaseFile(IFileSystemAccesor fs, string filename)
        {
            if (filename.IsBlank()) return string.Empty;

            var path = Path.Combine(fs.CurrentExeDir, filename);
            fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }


        protected virtual LiteDatabase CreateLiteDatabase()
        {
            if (IsInMemory)
                return new LiteDatabase(_memStream, _bMapr);
            
            if (DatabaseFullPath == null)
            {
                DatabaseFilename = GetDatabaseFilename();
                DatabaseFullPath = LocateDatabaseFile(_fs, DatabaseFilename);
            }

            return new LiteDatabase(ConnectString.LiteDB(DatabaseFullPath), _bMapr);
        }


        public virtual LiteDatabase ConnectToDB<T>(out LiteCollection<T> collection, string collectionName = null)
        {
            var db     = CreateLiteDatabase();
            collection = db.GetCollection<T>(collectionName ?? typeof(T).Name);
            return db;
        }
    }
}
