using System;

namespace Repo2.Core.ns11.DateTimeTools
{
    public class Today
    {
        public static DateTime FirstDayOfMonth()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, 1);
        }
    }
}
