using System;
using System.Collections.Generic;

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
    }
}
