using LiteDB;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using System.IO;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract partial class LocalRepoBase<T> : R2LiteRepoBase
    {
        private bool _isSeeded;

        public LocalRepoBase(IFileSystemAccesor fileSystemAccessor) : base(fileSystemAccessor)
        {
            TypeName         = typeof(T).Name;
            CollectionName   = GetCollectionName();
        }

        public string   TypeName           { get; }
        //public string   JsonSeedFilename   { get; private set; }
        //public string   JsonSeedFullPath   { get; private set; }
        public string   CollectionName     { get; }
        public bool     IsSeedEnabled      { get; protected set; }


        protected override string GetDatabaseFilename() => $"LocalRepo_{TypeName}.LiteDB3";
        protected virtual  string GetCollectionName  () => "v1";
        protected virtual  string GetJsonSeedFilename() => "seed.json";


        public override LiteDatabase ConnectToDB<TCol>(out LiteCollection<TCol> collection, string collectionName = null)
            => base.ConnectToDB(out collection, CollectionName);


        protected override void OnDBFileLocated(string databaseFullPath)
        {
            if (_fs == null) return;

            var jsonFilename = GetJsonSeedFilename();
            var jsonFullPath = LocateJsonSeedFile(_fs, DatabaseFullPath, jsonFilename);

            if (IsSeedEnabled && !_isSeeded)
            {
                //SetStatus($"DatabaseFullPath: {DatabaseFullPath} :  {_fs.Found(DatabaseFullPath)}");
                //SetStatus($"JsonSeedFullPath: {JsonSeedFullPath} :  {_fs.Found(JsonSeedFullPath)}");

                if (!_fs.Found(DatabaseFullPath)
                  && _fs.Found(jsonFullPath))
                    InsertSeedRecordsFromFile(jsonFilename, jsonFullPath);

                _isSeeded = true;
            }
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
