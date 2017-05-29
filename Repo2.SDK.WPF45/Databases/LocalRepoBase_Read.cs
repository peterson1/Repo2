using LiteDB;
using Repo2.SDK.WPF45.ChangeNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repo2.SDK.WPF45.Databases
{
    public abstract partial class LocalRepoBase<T> : R2LiteRepoBase
    {
        public virtual List<T> Find(Expression<Func<T, bool>> predicate)
        {
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                return col.Find(predicate).ToList();
            }
        }



        public virtual List<T> Find(Query query)
        {
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                return col.Find(query).ToList();
            }
        }



        public virtual List<T> FindAll()
        {
            List<T> list;
            SetStatus($"Querying ALL ‹{TypeName}› records ...");

            using (var db = ConnectToDB(out LiteCollection<T> col))
                list = col.FindAll().ToList();

            SetStatus($"Query returned {list.Count:N0} ‹{TypeName}› records.");
            return list;
        }


        public virtual T FindById(uint id)
        {
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                var rec = col.FindById((long)id);

                if (rec == null)
                    SetStatus($"‹{TypeName}› not found whose ID is [{id}].");

                return rec;
            }
        }


        public virtual uint CountAll()
        {
            using (var db = ConnectToDB(out LiteCollection<T> col))
            {
                return (uint)col.LongCount();
            }
        }


        public virtual List<T> FindByDates(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
