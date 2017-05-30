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


        public virtual List<T> FindAll<T>()
        {
            List<T> list;
            SetStatus($"Querying ALL ‹{typeof(T).Name}› records ...");

            using (var db = ConnectToDB(out LiteCollection<T> col))
                list = col.FindAll().ToList();

            SetStatus($"Query returned {list.Count:N0} ‹{typeof(T).Name}› records.");
            return list;
        }


        public virtual uint CountAll<T>()
        {
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                return (uint)col.LongCount();
            }
        }
    }
}
