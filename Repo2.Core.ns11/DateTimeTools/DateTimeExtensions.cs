using System;
using System.Collections.Generic;
using System.Linq;

namespace Repo2.Core.ns11.DateTimeTools
{
    public static class DateTimeExtensions
    {

        // thanks to http://stackoverflow.com/a/9176734/3973863
        public static IEnumerable<DateTime> EachDayUpTo(this DateTime start, DateTime end)
        {
            // Remove time info from start date (we only care about day). 
            var currentDay = new DateTime(start.Year, start.Month, start.Day);
            while (currentDay <= end)
            {
                yield return currentDay;
                currentDay = currentDay.AddDays(1);
            }
        }

        public static IEnumerable<DateTime> EachDayUpToNow(this DateTime start)
            => start.EachDayUpTo(DateTime.Now);



        public static IEnumerable<DateTime> EachMonthUpTo(this DateTime start, DateTime end)
            => start.EachDayUpTo       (end)
                    .Select            (x => new DateTime(x.Year, x.Month, 1))
                    .GroupBy           (x => x).Select(x => x.First())
                    .OrderByDescending (x => x).ToList();

        public static IEnumerable<DateTime> EachMonthUpToNow(this DateTime start)
            => start.EachMonthUpTo(DateTime.Now);


        public static IEnumerable<int> EachYearUpTo(this DateTime start, int endYear)
        {
            var yrs = new List<int>();
            for (int i = endYear; i >= start.Year; i--)
            {
                yrs.Add(i);
            }
            return yrs;
        }

        public static IEnumerable<int> EachYearUpToNow(this DateTime start)
            => start.EachYearUpTo(DateTime.Now.Year);

        public static IEnumerable<int> EachYearUpToLastYear(this DateTime start)
            => start.EachYearUpTo(DateTime.Now.Year - 1);
    }
}
