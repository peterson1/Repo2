using LiteDB;
using Repo2.Core.ns11.Exceptions;
using Repo2.SDK.WPF45.ChangeNotification;
using Repo2.SDK.WPF45.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract partial class LocalRepoBase<T> : StatusChangerN45
    {
        protected virtual void InsertSeedRecordsFromFile()
        {
            var jsonStr = File.ReadAllText(JsonSeedFullPath);
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


        public virtual uint Insert(T newRecord)
        {
            if (newRecord == null)
                throw Fault.NullRef<T>("record to insert");

            SetStatus($"Inserting new ‹{TypeName}› record ...");
            BsonValue bVal;
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                bVal = col.Insert(newRecord);
            }
            var id = (uint)bVal.AsInt64;
            SetStatus($"Sucessfully inserted ‹{TypeName}› (id: {id}).");
            return id;
        }


        public virtual uint BatchInsert(IEnumerable<T> newRecords)
        {
            SetStatus($"Inserting {newRecords.Count()} ‹{TypeName}› records ...");
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                using (var trans = db.BeginTrans())
                {
                    foreach (var rec in newRecords)
                    {
                        if (rec != null)
                            col.Insert(rec);
                    }

                    trans.Commit();
                }
            }
            var newCount = CountAll();
            SetStatus($"Successfully inserted {newRecords.Count()} ‹{TypeName}› records. New record count: [{newCount:N0}]");
            return newCount;
        }


        public virtual uint DeleteAll()
        {
            SetStatus($"Deleting all [{CountAll():N0}] ‹{TypeName}› records ...");
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                db.DropCollection(col.Name);
            }
            var newCount = CountAll();
            SetStatus($"DeleteAll ‹{TypeName}› completed. New record count: [{newCount:N0}].");
            return newCount;
        }


        protected void EnsureIndex<K>(Expression<Func<T, K>> indexExpression, bool isUnique)
        {
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                col.EnsureIndex<K>(indexExpression, isUnique);
            }
        }
    }
}
