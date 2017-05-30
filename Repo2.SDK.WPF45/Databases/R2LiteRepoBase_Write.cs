using LiteDB;
using Repo2.Core.ns11.Databases;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.SDK.WPF45.ChangeNotification;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract partial class R2LiteRepoBase : StatusChangerN45
    {


        public virtual uint BatchInsert<T>(IEnumerable<T> newRecords)
        {
            SetStatus($"Inserting {newRecords.Count()} ‹{typeof(T).Name}› records ...");
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                using (var trans = db.BeginTrans())
                {
                    foreach (var rec in newRecords)
                    {
                        if (rec == null) continue;

                        if (!PreInsertValidate(rec, col, out string msg))
                            throw new InvalidDataException(msg);

                        col.Insert(rec);
                    }
                    EnsureIndeces(col);
                    trans.Commit();
                }
            }
            var newCount = CountAll<T>();
            SetStatus($"Successfully inserted {newRecords.Count()} ‹{typeof(T).Name}› records. New record count: [{newCount:N0}]");
            return newCount;
        }


        protected virtual bool PreInsertValidate<T>(T newRecord, LiteCollection<T> col, out string msg)
        {
            msg = string.Empty;
            return true;
        }


        protected virtual void EnsureIndeces<T>(LiteCollection<T> col)
        {
        }
    }
}
