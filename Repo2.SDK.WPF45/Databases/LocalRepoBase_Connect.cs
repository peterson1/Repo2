using LiteDB;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using System.IO;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract partial class LocalRepoBase<T> : R2LiteDB
    {
        private bool _isSeeded;

        public LocalRepoBase(IFileSystemAccesor fileSystemAccessor) : base(fileSystemAccessor)
        {
            TypeName         = typeof(T).Name;
            CollectionName   = GetCollectionName();
            if (_fs != null)
            {
                JsonSeedFilename = GetJsonSeedFilename();
                JsonSeedFullPath = LocateJsonSeedFile(_fs, DatabaseFullPath, JsonSeedFilename);
            }
        }

        public string   TypeName           { get; }
        public string   JsonSeedFilename   { get; }
        public string   JsonSeedFullPath   { get; }
        public string   CollectionName     { get; }
        public bool     IsSeedEnabled      { get; protected set; }


        protected override string GetDatabaseFilename() => $"LocalRepo_{TypeName}.LiteDB3";
        protected virtual  string GetCollectionName  () => "v1";
        protected virtual  string GetJsonSeedFilename() => "seed.json";


        public override LiteDatabase ConnectToDB<TCol>(out LiteCollection<TCol> collection, string collectionName = null)
        {
            if (IsSeedEnabled && !_isSeeded)
            {
                if (!_fs.Found(DatabaseFullPath)
                  && _fs.Found(JsonSeedFullPath))
                    InsertSeedRecordsFromFile();

                _isSeeded = true;
            }
            return base.ConnectToDB(out collection, CollectionName);
        }


        protected virtual string LocateJsonSeedFile(IFileSystemAccesor fs, string dbFullPath, string jsonFilename)
        {
            if (dbFullPath.IsBlank()) return string.Empty;

            var path = Path.Combine(fs.ParentDir(dbFullPath), jsonFilename);
            fs.CreateDir(Path.GetDirectoryName(path));
            return path;
        }
    }
}
