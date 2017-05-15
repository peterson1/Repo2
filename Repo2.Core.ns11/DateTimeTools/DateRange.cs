using System;
using System.Collections;
using System.Collections.Generic;
using Repo2.Core.ns11.DataStructures;
using System.Linq;

namespace Repo2.Core.ns11.DateTimeTools
{
    public class DateRange : IRange<DateTime>, IEnumerable<DateTime>
    {
        public DateRange(DateTime startDate, DateTime endDate)
        {
            Start = startDate;
            End   = endDate;
        }

        public DateTime  Start  { get; }
        public DateTime  End    { get; }


        public bool Includes(IRange<DateTime> range)
            => (Start <= range.Start) && (range.End <= End);


        public bool Includes(DateTime value)
            => (Start <= value) && (value <= End);


        public List<DateTime> GroupByMonthAndYear()
        {
            var list   = new List<DateTime>();
            var byMoYr = this.GroupBy(x => $"{x.Month}-{x.Year}");

            foreach (var grp in byMoYr)
            {
                var d    = grp.First();
                var day1 = new DateTime(d.Year, d.Month, 1);
                list.Add(day1);
            }
            return list;
        }


        public override string ToString()
            => $"{Start:d MMM yyyy} to {End:d MMM yyyy}";


        public IEnumerator<DateTime> GetEnumerator()
            => Start.EachDayUpTo(End).GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

    }
}
